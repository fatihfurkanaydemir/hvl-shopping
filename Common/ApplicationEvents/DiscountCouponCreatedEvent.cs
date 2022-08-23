using Common.EventBus.Events;

namespace Common.ApplicationEvents;

public class DiscountCouponCreatedEvent: Event
{
  public string CouponCode { get; set; }
  public decimal Amount { get; set; }
}
