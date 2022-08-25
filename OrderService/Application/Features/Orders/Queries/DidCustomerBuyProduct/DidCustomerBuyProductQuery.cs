using OrderService.Application.Interfaces.Repositories;
using OrderService.Application.Wrappers;
using AutoMapper;
using MediatR;
using Common.SharedViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace OrderService.Application.Features.Orders.Queries.DidCustomerBuyProductQuery;

public class DidCustomerBuyProductQuery : IRequest<Response<bool>>
{
  public string IdentityId { get; set; }
  public int ProductId { get; set; }
}

public class DidCustomerBuyProductQueryHandler : IRequestHandler<DidCustomerBuyProductQuery, Response<bool>>
{
  private readonly IOrderRepositoryAsync _orderRepository;
  private readonly IMapper _mapper;
  public DidCustomerBuyProductQueryHandler(IOrderRepositoryAsync orderRepository, IMapper mapper)
  {
    _orderRepository = orderRepository;
    _mapper = mapper;
  }

  public async Task<Response<bool>> Handle(DidCustomerBuyProductQuery request, CancellationToken cancellationToken)
  {
    
    var didBuyProduct = await _orderRepository.DidCustomerBuyProduct(request.IdentityId, request.ProductId);

    return new Response<bool>(didBuyProduct, "Did buy product");
  }
}
