using Common.EventBus.Events;

namespace Common.ApplicationEvents;

public class TestEvent : Event
{
  public string message;
}
