using OrderService.Application.EventHandlers;
using Common.EventBus;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infrastructure.Persistence.Repositories;
using OrderService.Infrastructure.Persistence.Contexts;

// handlers
using OrderService.Application.Features.Orders.EventHandlers.OrderPaymentCompleted;
using OrderService.Application.Features.Orders.EventHandlers.CreateOrder;

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
    services.AddTransient<CreateOrderEventHandler>();
    services.AddTransient<OrderPaymentCompletedEventHandler>();

    services.AddTransient<IOrderRepositoryAsync, OrderRepositoryAsync>();
    services.AddDbContext<OrderDbContext>();
  }
}
