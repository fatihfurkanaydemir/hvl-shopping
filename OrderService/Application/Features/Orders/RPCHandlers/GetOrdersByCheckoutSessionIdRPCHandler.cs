using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using OrderService.Application.Interfaces.Repositories;
using Common.Wrappers;
using Common.SharedViewModels;
using AutoMapper;

namespace OrderService.Application.Features.Orders.RPCHandlers;

public class GetOrdersByCheckoutSessionIdRPCHandler : IRPCHandler<GetOrdersByCheckoutSessionIdRPC, Response<IEnumerable<OrderViewModel>>>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public GetOrdersByCheckoutSessionIdRPCHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task<Response<IEnumerable<OrderViewModel>>> Handle(GetOrdersByCheckoutSessionIdRPC rpc)
  {
    var orders = await _orderRepository.GetOrdersByCheckoutSessionId(rpc.CheckoutSessionId);

    var viewModels = new List<OrderViewModel>();

    foreach(var order in orders)
    {
      viewModels.Add(_mapper.Map<OrderViewModel>(order));
    }

    return new Response<IEnumerable<OrderViewModel>>(viewModels, "Orders");
  }
}
