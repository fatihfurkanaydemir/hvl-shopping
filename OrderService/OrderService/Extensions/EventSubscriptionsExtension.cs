using OrderService.Application.EventHandlers;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;

namespace OrderService.Extensions;

public static class EventSubscriptionsExtension
{
  public static void SubscribeEvents(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
    eventBus.Subscribe<TestEvent, TestEventHandler>();
  }
}
