using Common.EventBus;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Order Service
using OrderService.Application.EventHandlers;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Infrastructure.Persistence.Repositories;
using OrderService.Infrastructure.Persistence.Contexts;


// Discount Service
using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Infrastructure.Persistence.Repositories;
using DiscountService.Infrastructure.Persistence.Contexts;

// Event handlers
using OrderService.Application.Features.Orders.EventHandlers.OrderPaymentCompleted;
using OrderService.Application.Features.Orders.EventHandlers.CreateOrder;

// RPC handlers
using DiscountService.Application.Features.Test;
using DiscountService.Application.Features.Coupons.RPCHandlers;

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

    // Event Handlers
    services.AddTransient<TestEventHandler>();
    services.AddTransient<CreateOrderEventHandler>();
    services.AddTransient<OrderPaymentCompletedEventHandler>();

    // RPC Handlers
    services.AddTransient<TestRPCHandler>();
    services.AddTransient<GetAllCouponsRPCHandler>();
    services.AddTransient<GetUsedCouponsByCustomerIdentityIdRPCHandler>();
    services.AddTransient<UseCouponRPCHandler>();
    services.AddTransient<CanUseCouponRPCHandler>();
    services.AddTransient<CreateCouponRPCHandler>();

    services.AddTransient<IOrderRepositoryAsync, OrderRepositoryAsync>();
    services.AddDbContext<OrderDbContext>();

    services.AddTransient<ICouponRepositoryAsync, CouponRepositoryAsync>();
    services.AddTransient<IUsedCouponRepositoryAsync, UsedCouponRepositoryAsync>();
    services.AddDbContext<DiscountDbContext>();
  }
}
