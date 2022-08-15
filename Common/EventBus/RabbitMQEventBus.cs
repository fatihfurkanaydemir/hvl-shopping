using Common.EventBus.Events;
using Common.EventBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Common.EventBus;

public class RabbitMQEventBus: IEventBus
{
  private readonly Dictionary<string, List<Type>> _subscriptionMap;
  private readonly List<Type> _eventTypes;
  private readonly IServiceScopeFactory _serviceScopeFactory;
  private readonly IConfiguration _configuration;

  public RabbitMQEventBus(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
  {
    _serviceScopeFactory = serviceScopeFactory;
    _subscriptionMap = new Dictionary<string, List<Type>>();
    _eventTypes = new List<Type>();
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
