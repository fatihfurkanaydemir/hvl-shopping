using Common.EventBus.Interfaces;
using Common.ApplicationEvents;

namespace OrderService.Application.EventHandlers
{
  public class TestEventHandler : IEventHandler<TestEvent>
  {
    public Task Handle(TestEvent @event)
    {
      Console.WriteLine(@event.message);

      return Task.CompletedTask;
    }
  }
}
