using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;

namespace Application.Features.Customers.Commands.UpdateCustomer
{
  public class UpdateCustomerCommand : IRequest<Response<string>>
  {

    [Required]
    public string IdentityId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Phone]
    [MinLength(10)]
    public string PhoneNumber { get; set; }
  }

  public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Response<string>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public UpdateCustomerCommandHandler(ICustomerRepositoryAsync customerRepository, AuthService authService, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _authService = authService;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.IdentityId);
      if (customer == null) throw new ApiException("Customer not found");

      customer.FirstName = request.FirstName;
      customer.LastName = request.LastName;
      customer.PhoneNumber = request.PhoneNumber;

      await _customerRepository.UpdateAsync(customer);

      return new Response<string>(customer.IdentityId, "Customer updated");
    }
  }
}
