using Common.EventBus.Events;

namespace Common.EventBus.Interfaces;

public interface IEventHandler<TEvent> where TEvent : Event
{
  Task Handle(TEvent @event);
}
