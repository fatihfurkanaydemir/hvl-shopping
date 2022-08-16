using Common.Entities;
using Common.Enums;
using Common.EventBus.Events;

namespace Common.ApplicationEvents;

public class OrderPaymentCompletedEvent : Event
{
  public string CheckoutSessionId { get; set; }
  public string PaymentIntentId { get; set; }
}
