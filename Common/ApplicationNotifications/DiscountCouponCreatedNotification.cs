using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ApplicationNotifications
{
  public class DiscountCouponCreatedNotification
  {
    public string CouponCode { get; set; }
    public decimal Amount { get; set; }
  }
}
