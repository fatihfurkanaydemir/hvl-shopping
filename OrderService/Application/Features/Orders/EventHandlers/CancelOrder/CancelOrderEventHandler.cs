using OrderService.Application.Interfaces.Repositories;
using AutoMapper;
using OrderService.Domain.Entities;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;

namespace OrderService.Application.Features.Orders.EventHandlers.CancelOrder;

public class CancelOrderEventHandler : IEventHandler<CancelOrderEvent>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public CancelOrderEventHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task Handle(CancelOrderEvent @event)
  {
    var order = await _orderRepository.GetByIdAsync(@event.OrderId);

    order.Status = Common.Enums.OrderStatus.Canceled;

    await _orderRepository.UpdateAsync(order);
  }
}
