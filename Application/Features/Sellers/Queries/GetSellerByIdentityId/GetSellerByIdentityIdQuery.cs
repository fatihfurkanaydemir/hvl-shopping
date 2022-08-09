using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Services;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Sellers.Queries.GetSellerByIdentityId
{
  public class GetSellerByIdentityIdQuery : IRequest<Response<GetSellerByIdentityIdViewModel>>
  {
    public string IdentityId { get; set; }
  }

  public class GetSellerByIdentityIdQueryHandler : IRequestHandler<GetSellerByIdentityIdQuery, Response<GetSellerByIdentityIdViewModel>>
  {
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public GetSellerByIdentityIdQueryHandler(ISellerRepositoryAsync sellerRepository, AuthService authService, IMapper mapper)
    {
      _sellerRepository = sellerRepository;
      _authService = authService;
      _mapper = mapper;
    }

    public async Task<Response<GetSellerByIdentityIdViewModel>> Handle(GetSellerByIdentityIdQuery request, CancellationToken cancellationToken)
    {
      var seller = await _sellerRepository.GetByIdentityIdAsync(request.IdentityId);
      if (seller == null) throw new ApiException("Seller not found");

      var sellerViewModel = _mapper.Map<GetSellerByIdentityIdViewModel>(seller);

      var profileResponse = await _authService.GetProfileInformation(request.IdentityId);
      if (!profileResponse.Succeeded) throw new ApiException("Could not get profile information");

      var profileInfo = profileResponse.Data;

      sellerViewModel.Email = profileInfo.Email;

      return new Response<GetSellerByIdentityIdViewModel>(sellerViewModel);
    }
  }
}
