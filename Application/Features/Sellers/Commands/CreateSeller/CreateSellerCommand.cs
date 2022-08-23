using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Application.Services;
using Application.DTOs;
using Application.Exceptions;

namespace Application.Features.Sellers.Commands.CreateSeller
{
  public class CreateSellerCommand : IRequest<Response<string>>
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
    
    // Seller related
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Phone]
    [MinLength(10)]
    public string PhoneNumber { get; set; }
    public string ShopName { get; set; }
    public AddressDTO Address { get; set; }
  }

  public class CreateSellerCommandHandler : IRequestHandler<CreateSellerCommand, Response<string>>
  {
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public CreateSellerCommandHandler(ISellerRepositoryAsync sellerRepository, AuthService authService, IMapper mapper)
    {
      _sellerRepository = sellerRepository;
      _authService = authService;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(CreateSellerCommand request, CancellationToken cancellationToken)
    {
      var seller = _mapper.Map<Seller>(request);
      seller.Address.Title = "Dükkan Adresi";

      var registerResponse = await _authService.RegisterSeller(request.Email, request.Password, request.ConfirmPassword);
      if (!registerResponse.Succeeded)
      {
        var exception = new ApiException(registerResponse.Message) { Errors = registerResponse.Errors };
        exception.Data["DataMessage"] = registerResponse.Data;
        throw exception;
      }

      seller.IdentityId = registerResponse.Data;

      await _sellerRepository.AddAsync(seller);

      return new Response<string>(seller.Id.ToString(), "Seller registered");
    }
  }
}
