using Common.EventBus.Events;
using Common.EventBus.RPCs;
using Common.EventBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Collections.Concurrent;

namespace Common.EventBus;

public class RabbitMQEventBus: IEventBus
{
  private readonly Dictionary<string, List<Type>> _subscriptionMap;
  private readonly Dictionary<string, Type> _rpcSubscriptionMap;
  private readonly Dictionary<Type, BlockingCollection<string>> _rpcResponseQueueMap;
  private readonly Dictionary<Type, IBasicProperties> _rpcPropsMap;
  private readonly List<Type> _eventTypes;
  private readonly Dictionary<Type, Type> _rpcTypes;
  private readonly IServiceScopeFactory _serviceScopeFactory;
  private readonly IConfiguration _configuration;

  public RabbitMQEventBus(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
  {
    _serviceScopeFactory = serviceScopeFactory;
    _subscriptionMap = new Dictionary<string, List<Type>>();
    _rpcSubscriptionMap = new Dictionary<string, Type>();
    _rpcResponseQueueMap = new Dictionary<Type, BlockingCollection<string>>();
    _rpcPropsMap = new Dictionary<Type, IBasicProperties>();
    _eventTypes = new List<Type>();
    _rpcTypes = new Dictionary<Type, Type>();
    _configuration = configuration;
  }

  public void Publish<T>(T @event) where T : Event
  {
    var factory = new ConnectionFactory() { 
      HostName = _configuration.GetSection("RabbitMQ")["HostName"] 
    };

    using(var connection = factory.CreateConnection())
    {
      using(var channel = connection.CreateModel())
      {
        var eventName = @event.GetType().Name;
        channel.QueueDeclare(eventName, false, false, false, null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", eventName, null, body);
      }
    }
  }

  public async Task<TRPCResult> CallRP<TRPC, TRPCResult>(TRPC rpc) where TRPC : RPC
  {
    var rpcType = rpc.GetType();

    if (!_rpcPropsMap.ContainsKey(rpcType) || !_rpcResponseQueueMap.ContainsKey(rpcType))
    {
      throw new ArgumentException($"RPC type {rpcType.Name} was not registered", rpcType.Name);
    }

    var factory = new ConnectionFactory()
    {
      HostName = _configuration.GetSection("RabbitMQ")["HostName"],
      DispatchConsumersAsync = true
    };

    using (var connection = factory.CreateConnection())
    {
      using (var channel = connection.CreateModel())
      {

        var message = JsonConvert.SerializeObject(rpc);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", rpcType.Name, basicProperties: _rpcPropsMap[rpcType], body: body);

        return JsonConvert.DeserializeObject<TRPCResult>(_rpcResponseQueueMap[rpcType].Take())!;
      }
    }
  }

  public async void RegisterRPC<TRPC, TRPCResult>() where TRPC : RPC
  {
    var factory = new ConnectionFactory()
    {
      HostName = _configuration.GetSection("RabbitMQ")["HostName"],
      DispatchConsumersAsync = true
    };

    var rpcType = typeof(TRPC);
    var rpcName = rpcType.Name;
    var rpcResultType = typeof(TRPCResult);

    if (!_rpcResponseQueueMap.ContainsKey(rpcType))
    {
      _rpcResponseQueueMap[rpcType] = new BlockingCollection<string>();
    }

    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    var replyQueueName = channel.QueueDeclare(rpcName + "Response", false, false, false, null).QueueName;
    var consumer = new AsyncEventingBasicConsumer(channel);

    var props = channel.CreateBasicProperties();
    var correlationId = Guid.NewGuid().ToString();
    props.CorrelationId = correlationId;
    props.ReplyTo = replyQueueName;

    if (!_rpcPropsMap.ContainsKey(rpcType))
    {
      _rpcPropsMap.Add(rpcType, props);
    }

    channel.BasicConsume(replyQueueName, true, consumer);

    consumer.Received += async (sender, ea) =>
    {
      var body = ea.Body.ToArray();
      var response = Encoding.UTF8.GetString(body);
      if (ea.BasicProperties.CorrelationId == correlationId)
      {
        _rpcResponseQueueMap[rpcType].Add(response);
      }
    };
  }

  public void RegisterRPCHandler<TRPC, TRPCHandler, TRPCResult>()
    where TRPC : RPC
    where TRPCHandler : IRPCHandler<TRPC, TRPCResult>
  {
    var rpcType = typeof(TRPC);
    var rpcName = rpcType.Name;
    var rpcResultType = typeof(TRPCResult);
    var handlerType = typeof(TRPCHandler);

    if(!_rpcTypes.ContainsKey(rpcType))
    {
      _rpcTypes.Add(rpcType, rpcResultType);
    }

    if(_rpcSubscriptionMap.ContainsKey(rpcName))
    {
      throw new ArgumentException($"RPC handler type {handlerType.Name} was already registered for '{rpcName}'", handlerType.Name);
    }

    _rpcSubscriptionMap.Add(rpcName, handlerType);

    var factory = new ConnectionFactory()
    {
      HostName = _configuration.GetSection("RabbitMQ")["HostName"],
      DispatchConsumersAsync = true
    };

    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    channel.QueueDeclare(rpcName, false, false, false, null);

    var consumer = new AsyncEventingBasicConsumer(channel);
    channel.BasicConsume(rpcName, false, consumer);

    consumer.Received += async (sender, ea) =>
    {
      string response = null;

      var rpcName = ea.RoutingKey;

      if (!_rpcSubscriptionMap.ContainsKey(rpcName)) return;

      var props = ea.BasicProperties;
      var replyProps = channel.CreateBasicProperties();
      replyProps.CorrelationId = props.CorrelationId;

      try
      {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());

        using (var scope = _serviceScopeFactory.CreateScope())
        {
          var subscription = _rpcSubscriptionMap[rpcName];
          
          var handler = scope.ServiceProvider.GetService(subscription);
          if (handler == null) throw new ArgumentException($"RPC handler type {handlerType.Name} was not found for '{rpcName}'", handlerType.Name);

          var rpcType = _rpcTypes.SingleOrDefault(t => t.Key.Name == rpcName);
          var rpc = JsonConvert.DeserializeObject(message, rpcType.Key);
          var concreteType = typeof(IRPCHandler<,>).MakeGenericType(rpcType.Key, rpcType.Value);

          var method = concreteType.GetMethod("Handle", new Type[] { rpcType.Key });

          var result = await (Task<TRPCResult>)method!.Invoke(handler, new object[] { rpc! })!;
          response = JsonConvert.SerializeObject(result); 
          
        }

      } catch (Exception ex)
      {
        response = "";
      }
      finally
      {
        var responseBytes = Encoding.UTF8.GetBytes(response);

        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
      }
    };
  }

  public void Subscribe<T, TH>() 
    where T: Event
    where TH : IEventHandler<T>
  {
    var eventType = typeof(T);
    var eventName = eventType.Name;
    var handlerType = typeof(TH);

    if(!_eventTypes.Contains(eventType))
    {
      _eventTypes.Add(eventType);
    }

    if(!_subscriptionMap.ContainsKey(eventName))
    {
      _subscriptionMap.Add(eventName, new List<Type>());
    }

    if(_subscriptionMap[eventName].Any(s => s.GetType() == handlerType))
    {
      throw new ArgumentException($"Event handler type {handlerType.Name} was already registered for '{eventName}'", handlerType.Name);
    }

    _subscriptionMap[eventName].Add(handlerType);

    StartBasicConsume<T>();
  }

  private void StartBasicConsume<T>() where T : Event
  {
    var factory = new ConnectionFactory() { 
      HostName = _configuration.GetSection("RabbitMQ")["HostName"], 
      DispatchConsumersAsync = true 
    };

    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    var eventName = typeof(T).Name;
    channel.QueueDeclare(eventName, false, false, false, null);

    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.Received += Consumer_Received;

    channel.BasicConsume(eventName, true, consumer);
  }

  private async Task Consumer_Received (object sender, BasicDeliverEventArgs ea)
  {
    var eventName = ea.RoutingKey;
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

    await ProcessEvent(eventName, message);
  }

  private async Task ProcessEvent(string eventName, string message)
  {
    if (!_subscriptionMap.ContainsKey(eventName)) return;

    using (var scope = _serviceScopeFactory.CreateScope())
    {
      var subscriptions = _subscriptionMap[eventName];
      foreach(var subscription in subscriptions)
      {
        var handler = scope.ServiceProvider.GetService(subscription);
        if(handler == null) continue;

        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
        var @event = JsonConvert.DeserializeObject(message, eventType);
        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

        var method = concreteType.GetMethod("Handle", new Type[] { eventType });

        await (Task)method!.Invoke(handler, new object[] { @event! })!;
      }
    }
  }
}
