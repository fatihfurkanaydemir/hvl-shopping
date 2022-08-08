using Application.Exceptions;
using Application.Interfaces.Repositories;
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
    private readonly IMapper _mapper;
    public GetSellerByIdentityIdQueryHandler(ISellerRepositoryAsync sellerRepository, IMapper mapper)
    {
      _sellerRepository = sellerRepository;
      _mapper = mapper;
    }

    public async Task<Response<GetSellerByIdentityIdViewModel>> Handle(GetSellerByIdentityIdQuery request, CancellationToken cancellationToken)
    {
      var seller = await _sellerRepository.GetByIdentityIdAsync(request.IdentityId);
      if (seller == null) throw new ApiException("Seller not found");

      var sellerViewModel = _mapper.Map<GetSellerByIdentityIdViewModel>(seller);

      return new Response<GetSellerByIdentityIdViewModel>(sellerViewModel);
    }
  }
}
