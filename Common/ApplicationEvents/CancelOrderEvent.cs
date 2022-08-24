using Common.Entities;
using Common.Enums;
using Common.EventBus.Events;

namespace Common.ApplicationEvents;

public class CancelOrderEvent : Event
{
  public int OrderId { get; set; }
}
