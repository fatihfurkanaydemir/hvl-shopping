using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;

namespace Application.Features.Customers.Commands.DeleteAddress
{
  public class DeleteAddressCommand : IRequest<Response<string>>
  {
    [Required]
    public string IdentityId { get; set; }
    [Required]
    public int AddressId { get; set; }
  }

  public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Response<string>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IAddressRepositoryAsync _addressRepository;
    private readonly IMapper _mapper;
    public DeleteAddressCommandHandler(ICustomerRepositoryAsync customerRepository, IAddressRepositoryAsync addressRepository, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _addressRepository = addressRepository;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.IdentityId);
      if (customer == null) throw new ApiException("Customer not found");

      var address = customer.Addresses.Find(a => a.Id == request.AddressId);
      if (address == null) throw new ApiException("Address not found");

      await _addressRepository.DeleteAsync(address);

      return new Response<string>(address.Id.ToString(), "Address deleted");
    }
  }
}
