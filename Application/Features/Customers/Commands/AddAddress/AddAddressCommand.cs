using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;

namespace Application.Features.Customers.Commands.AddAddress
{
  public class AddAddressCommand : IRequest<Response<string>>
  {
    [Required]
    public string Title { get; set; }
    [Required]
    public string IdentityId { get; set; }
    [Required]
    public string AddressDescription { get; set; }
    [Required]
    public string City { get; set; }
  }

  public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, Response<string>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IMapper _mapper;
    public AddAddressCommandHandler(ICustomerRepositoryAsync customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.IdentityId);
      if (customer == null) throw new ApiException("Customer not found");

      var address = new Address
      {
        Title = request.Title,
        AddressDescription = request.AddressDescription,
        City = request.City,
      };

      await _customerRepository.MarkUnchangedAsync(customer);
      customer.Addresses.Add(address);

      await _customerRepository.UpdateAsync(customer);

      return new Response<string>(customer.IdentityId, "Address added");
    }
  }
}
