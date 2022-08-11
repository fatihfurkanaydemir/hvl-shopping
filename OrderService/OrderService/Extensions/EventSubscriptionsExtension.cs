using OrderService.Application.EventHandlers;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using OrderService.Application.Features.Orders.EventHandlers.CreateOrder;

namespace OrderService.Extensions;

public static class EventSubscriptionsExtension
{
  public static void SubscribeEvents(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<TestEvent, TestEventHandler>();
    eventBus.Subscribe<CreateOrderEvent, CreateOrderEventHandler>();
  }
}
