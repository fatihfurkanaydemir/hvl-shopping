using OrderService.Application.EventHandlers;
using Common.EventBus;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GlobalInfrastructure;

public static class DependencyExtension
{
  public static void AddGlobalInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
    {
      var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
      return new RabbitMQEventBus(scopeFactory, configuration);
    });

    services.AddTransient<TestEventHandler>();
    //services.AddTransient<IEventHandler<TestEvent>, TestEventHandler>();
  }
}
