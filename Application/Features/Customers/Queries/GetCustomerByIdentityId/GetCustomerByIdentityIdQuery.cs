using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomerByIdentityId
{
  public class GetCustomerByIdentityIdQuery : IRequest<Response<GetCustomerByIdentityIdViewModel>>
  {
    public string identityId { get; set; }
  }

  public class GetCustomerByIdentityIdQueryHandler : IRequestHandler<GetCustomerByIdentityIdQuery, Response<GetCustomerByIdentityIdViewModel>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IMapper _mapper;
    public GetCustomerByIdentityIdQueryHandler(ICustomerRepositoryAsync customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _mapper = mapper;
    }

    public async Task<Response<GetCustomerByIdentityIdViewModel>> Handle(GetCustomerByIdentityIdQuery request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.identityId);
      if (customer == null) throw new ApiException("Customer not found");

      var customerViewModel = _mapper.Map<GetCustomerByIdentityIdViewModel>(customer);

      return new Response<GetCustomerByIdentityIdViewModel>(customerViewModel);
    }
  }
}
