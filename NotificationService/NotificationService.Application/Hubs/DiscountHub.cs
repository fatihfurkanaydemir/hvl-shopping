using Common.ApplicationNotifications;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Interfaces.Hubs;

namespace NotificationService.Application.Hubs;

public class DiscountHub: Hub<IDiscountHub>
{
  public async Task Notify_DiscountCouponCreated(DiscountCouponCreatedNotification notification)
  {
    await Clients.All.Notify_DiscountCouponCreated(notification);
  }

  public async Task JoinGroup(string groupName)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
  }

  public async Task LeaveGroup(string groupName)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
  }
}
