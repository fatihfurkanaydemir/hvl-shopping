using Common.ApplicationNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces.Hubs;

public interface IDiscountHub
{
  public Task Notify_DiscountCouponCreated(DiscountCouponCreatedNotification notification);
}
