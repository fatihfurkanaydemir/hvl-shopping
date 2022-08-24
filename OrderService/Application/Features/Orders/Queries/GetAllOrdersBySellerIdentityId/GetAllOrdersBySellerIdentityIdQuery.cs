using OrderService.Application.Interfaces.Repositories;
using OrderService.Application.Wrappers;
using AutoMapper;
using MediatR;
using Common.SharedViewModels;

namespace OrderService.Application.Features.Orders.Queries.GetAllOrdersBySellerIdentityId;

public class GetAllOrdersBySellerIdentityIdQuery : IRequest<PagedResponse<IEnumerable<OrderViewModel>>>
{
  public string IdentityId { get; set; }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
}

public class GetAllOrdersBySellerIdentityIdQueryHandler : IRequestHandler<GetAllOrdersBySellerIdentityIdQuery, PagedResponse<IEnumerable<OrderViewModel>>>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public GetAllOrdersBySellerIdentityIdQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task<PagedResponse<IEnumerable<OrderViewModel>>> Handle(GetAllOrdersBySellerIdentityIdQuery request, CancellationToken cancellationToken)
  {
    var validFilter = _mapper.Map<GetAllOrdersBySellerIdentityIdParameter>(request);
    var dataCount = await _orderRepository.GetDataCountBySellerIdentityIdAsync(request.IdentityId);
    var orders = await _orderRepository.GetAllOrdersBySellerIdentityIdAsync(request.IdentityId, request.PageNumber, request.PageSize);

    var orderViewModels = new List<OrderViewModel>();

    foreach (var o in orders)
    {
      var order = _mapper.Map<OrderViewModel>(o);

      orderViewModels.Add(order);
    }

    return new PagedResponse<IEnumerable<OrderViewModel>>(orderViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
  }
}
