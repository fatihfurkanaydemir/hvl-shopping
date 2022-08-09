using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;

namespace Application.Features.Customers.Commands.UpdateAddress
{
  public class UpdateAddressCommand : IRequest<Response<string>>
  {
    [Required]
    public string IdentityId { get; set; }
    [Required]
    public int AddressId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string AddressDescription { get; set; }
    [Required]
    public string City { get; set; }
  }

  public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Response<string>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IAddressRepositoryAsync _addressRepository;
    private readonly IMapper _mapper;
    public UpdateAddressCommandHandler(ICustomerRepositoryAsync customerRepository, IAddressRepositoryAsync addressRepository, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _addressRepository = addressRepository;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.IdentityId);
      if (customer == null) throw new ApiException("Customer not found");

      var address = customer.Addresses.Find(a => a.Id == request.AddressId);
      if (address == null) throw new ApiException("Address not found");

      address.AddressDescription = request.AddressDescription;
      address.Title = request.Title;
      address.City = request.City;

      await _addressRepository.UpdateAsync(address);

      return new Response<string>(address.Id.ToString(), "Address updated");
    }
  }
}
