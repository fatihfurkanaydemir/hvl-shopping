using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Sellers.Queries.GetAllSellers
{
  public class GetAllSellersQuery : IRequest<PagedResponse<IEnumerable<GetAllSellersViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetAllSellersQueryHandler : IRequestHandler<GetAllSellersQuery, PagedResponse<IEnumerable<GetAllSellersViewModel>>>
  {
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly IMapper _mapper;
    public GetAllSellersQueryHandler(ISellerRepositoryAsync sellerRepository, IMapper mapper)
    {
      _sellerRepository = sellerRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllSellersViewModel>>> Handle(GetAllSellersQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllSellersParameter>(request);
      var dataCount = await _sellerRepository.GetDataCount();
      var sellers = await _sellerRepository.GetPageResponseWithRelationsAsync(request.PageNumber, request.PageSize);

      var sellerViewModels = new List<GetAllSellersViewModel>();

      foreach (var s in sellers)
      {
        var seller = _mapper.Map<GetAllSellersViewModel>(s);

        sellerViewModels.Add(seller);
      }

      return new PagedResponse<IEnumerable<GetAllSellersViewModel>>(sellerViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
    }
  }
}