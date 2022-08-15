using OrderService.Application.Interfaces.Repositories;
using OrderService.Application.Wrappers;
using AutoMapper;
using MediatR;
using OrderService.Application.Features.SharedViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace OrderService.Application.Features.Orders.Queries.GetAllOrdersByCustomerIdentityId;

public class GetAllOrdersByCustomerIdentityIdQuery : IRequest<PagedResponse<IEnumerable<OrderViewModel>>>
{
  public string IdentityId { get; set; }
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
}

public class GetAllOrdersByCustomerIdentityIdQueryHandler : IRequestHandler<GetAllOrdersByCustomerIdentityIdQuery, PagedResponse<IEnumerable<OrderViewModel>>>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public GetAllOrdersByCustomerIdentityIdQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task<PagedResponse<IEnumerable<OrderViewModel>>> Handle(GetAllOrdersByCustomerIdentityIdQuery request, CancellationToken cancellationToken)
  {
    var validFilter = _mapper.Map<GetAllOrdersByCustomerIdentityIdParameter>(request);
    var dataCount = await _orderRepository.GetDataCountByCustomerIdentityIdAsync(request.IdentityId);
    var orders = await _orderRepository.GetAllOrdersByCustomerIdentityIdAsync(request.IdentityId, request.PageNumber, request.PageSize);

    var orderViewModels = new List<OrderViewModel>();

    foreach (var o in orders)
    {
      var order = _mapper.Map<OrderViewModel>(o);

      orderViewModels.Add(order);
    }

    return new PagedResponse<IEnumerable<OrderViewModel>>(orderViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
  }
}
