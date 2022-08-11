using OrderService.Application.Interfaces.Repositories;
using OrderService.Application.Wrappers;
using AutoMapper;
using MediatR;
using OrderService.Application.Features.SharedViewModels;

namespace OrderService.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQuery : IRequest<PagedResponse<IEnumerable<OrderViewModel>>>
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
}

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResponse<IEnumerable<OrderViewModel>>>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public GetAllOrdersQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task<PagedResponse<IEnumerable<OrderViewModel>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
  {
    var validFilter = _mapper.Map<GetAllOrdersParameter>(request);
    var dataCount = await _orderRepository.GetDataCount();
    var orders = await _orderRepository.GetPagedReponseWithRelationsAsync(request.PageNumber, request.PageSize);

    var orderViewModels = new List<OrderViewModel>();

    foreach (var o in orders)
    {
      var order = _mapper.Map<OrderViewModel>(o);

      orderViewModels.Add(order);
    }

    return new PagedResponse<IEnumerable<OrderViewModel>>(orderViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
  }
}
