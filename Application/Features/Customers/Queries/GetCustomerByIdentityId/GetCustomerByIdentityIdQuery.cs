using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Services;
using Application.Wrappers;
using AutoMapper;
using Domain.Common;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.Customers.Queries.GetCustomerByIdentityId
{
  public class GetCustomerByIdentityIdQuery : IRequest<Response<GetCustomerByIdentityIdViewModel>>
  {
    public string identityId { get; set; }
  }

  public class GetCustomerByIdentityIdQueryHandler : IRequestHandler<GetCustomerByIdentityIdQuery, Response<GetCustomerByIdentityIdViewModel>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public GetCustomerByIdentityIdQueryHandler(ICustomerRepositoryAsync customerRepository, AuthService authService, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _authService = authService;
      _mapper = mapper;
    }

    public async Task<Response<GetCustomerByIdentityIdViewModel>> Handle(GetCustomerByIdentityIdQuery request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.identityId);
      if (customer == null) throw new ApiException("Customer not found");

      var customerViewModel = _mapper.Map<GetCustomerByIdentityIdViewModel>(customer);

      var profileResponse = await _authService.GetProfileInformation(request.identityId);
      if (!profileResponse.Succeeded) throw new ApiException("Could not get profile information");

      var profileInfo = profileResponse.Data;

      customerViewModel.Email = profileInfo.Email;

      return new Response<GetCustomerByIdentityIdViewModel>(customerViewModel);
    }
  }
}
