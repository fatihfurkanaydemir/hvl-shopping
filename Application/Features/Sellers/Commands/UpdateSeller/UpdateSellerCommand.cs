using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;

namespace Application.Features.Sellers.Commands.UpdateSeller
{
  public class UpdateSellerCommand : IRequest<Response<string>>
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
    [Required]
    public string ShopName { get; set; }
    [Required]
    public Address Address { get; set; }
  }

  public class UpdateSellerCommandHandler : IRequestHandler<UpdateSellerCommand, Response<string>>
  {
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly IAddressRepositoryAsync _addressRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public UpdateSellerCommandHandler(ISellerRepositoryAsync sellerRepository, IAddressRepositoryAsync addressRepository, AuthService authService, IMapper mapper)
    {
      _sellerRepository = sellerRepository;
      _addressRepository = addressRepository;
      _authService = authService;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(UpdateSellerCommand request, CancellationToken cancellationToken)
    {
      var seller = await _sellerRepository.GetByIdentityIdAsync(request.IdentityId);
      if (seller == null) throw new ApiException("Seller not found");

      seller.FirstName = request.FirstName;
      seller.LastName = request.LastName;
      seller.PhoneNumber = request.PhoneNumber;
      seller.ShopName = request.ShopName;


      seller.Address.AddressDescription = request.Address.AddressDescription;
      seller.Address.City = request.Address.City;

      await _sellerRepository.UpdateAsync(seller);
      await _addressRepository.UpdateAsync(seller.Address);

      return new Response<string>(seller.IdentityId, "Seller updated");
    }
  }
}
