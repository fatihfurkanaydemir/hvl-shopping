using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;

namespace Application.Features.Customers.Commands.CreateCustomer
{
  public class CreateCustomerCommand : IRequest<Response<string>>
  {
    // Will be redirected to AuthServer
    // Identity related
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    
    // Customer related
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Phone]
    [MinLength(10)]
    public string PhoneNumber { get; set; }
  }

  public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<string>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public CreateCustomerCommandHandler(ICustomerRepositoryAsync customerRepository, AuthService authService, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _authService = authService;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
      var customer = _mapper.Map<Customer>(request);

      var registerResponse = await _authService.RegisterCustomer(request.Email, request.Password, request.ConfirmPassword);
      if (!registerResponse.Succeeded) return registerResponse;

      customer.IdentityId = registerResponse.Data;
      customer.Addresses = new List<Address>();

      await _customerRepository.AddAsync(customer);

      return new Response<string>(customer.IdentityId, "Customer registered");
    }
  }
}
