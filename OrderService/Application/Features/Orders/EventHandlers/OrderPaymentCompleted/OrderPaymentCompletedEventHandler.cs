using OrderService.Application.Interfaces.Repositories;
using AutoMapper;
using OrderService.Domain.Entities;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using OrderService.Application.Exceptions;

namespace OrderService.Application.Features.Orders.EventHandlers.OrderPaymentCompleted;

public class OrderPaymentCompletedEventHandler : IEventHandler<OrderPaymentCompletedEvent>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public OrderPaymentCompletedEventHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task Handle(OrderPaymentCompletedEvent @event)
  {
    var orders = await _orderRepository.GetOrdersByCheckoutSessionId(@event.CheckoutSessionId);
    if (orders.Count == 0) throw new ApiException("Orders not found");

    foreach(var order in orders)
    {
      order.PaymentIntentId = @event.PaymentIntentId;
      order.Status = Common.Enums.OrderStatus.AwaitingShipment;
      await _orderRepository.UpdateAsync(order);
    }
  }
}
