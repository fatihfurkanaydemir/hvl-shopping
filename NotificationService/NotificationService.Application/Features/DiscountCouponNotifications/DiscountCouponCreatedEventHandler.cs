using Common.ApplicationEvents;
using Common.ApplicationNotifications;
using Common.EventBus.Interfaces;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Hubs;
using NotificationService.Application.Interfaces.Hubs;

namespace NotificationService.Application.Features.DiscountCouponNotifications;

public class DiscountCouponCreatedEventHandler: IEventHandler<DiscountCouponCreatedEvent>
{
  private readonly IHubContext<DiscountHub, IDiscountHub> _hub;
  public DiscountCouponCreatedEventHandler(IHubContext<DiscountHub, IDiscountHub> hub)
  {
    _hub = hub;
  }
    
  public async Task Handle(DiscountCouponCreatedEvent @event)
  {
    await _hub.Clients.All.Notify_DiscountCouponCreated(new DiscountCouponCreatedNotification
    {
      CouponCode = @event.CouponCode,
      Amount = @event.Amount,
    });
  }
}
