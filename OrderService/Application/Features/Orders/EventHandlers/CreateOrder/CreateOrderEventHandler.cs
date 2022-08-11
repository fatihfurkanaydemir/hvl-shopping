using OrderService.Application.Interfaces.Repositories;
using AutoMapper;
using OrderService.Domain.Entities;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;

namespace OrderService.Application.Features.Orders.EventHandlers.CreateOrder;

public class CreateOrderEventHandler : IEventHandler<CreateOrderEvent>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public CreateOrderEventHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task Handle(CreateOrderEvent @event)
  {
    var order = _mapper.Map<Order>(@event);

    await _orderRepository.AddAsync(order);
  }
}
