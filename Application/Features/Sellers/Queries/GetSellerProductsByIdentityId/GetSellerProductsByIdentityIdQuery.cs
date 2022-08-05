using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Application.Features.SharedViewModels;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Sellers.Queries.GetSellerProductsByIdentityId
{
  public class GetSellerProductsByIdentityIdQuery : IRequest<PagedResponse<GetSellerProductsByIdentityIdViewModel>>
  {
    public string? IdentityId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetSellerProductsByIdentityIdQueryHandler : IRequestHandler<GetSellerProductsByIdentityIdQuery, PagedResponse<GetSellerProductsByIdentityIdViewModel>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly IMapper _mapper;
    public GetSellerProductsByIdentityIdQueryHandler(ISellerRepositoryAsync sellerRepository, IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _sellerRepository = sellerRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<GetSellerProductsByIdentityIdViewModel>> Handle(GetSellerProductsByIdentityIdQuery request, CancellationToken cancellationToken)
    {
      var seller = await _sellerRepository.GetByIdentityIdAsync(request.IdentityId);
      if (seller == null) throw new ApiException("Seller not found");

      var validFilter = _mapper.Map<GetSellerProductsByIdentityIdParameter>(request);
      var dataCount = await _productRepository.GetDataCountBySellerIdentityIdAsync(request.IdentityId);

      var products = await _productRepository.GetBySellerIdentityIdWithRelationsAsync(request.IdentityId, validFilter.PageNumber, validFilter.PageSize);

      var productViewModels = new List<ProductViewModel>();
      foreach (Product product in products)
      {
        productViewModels.Add(_mapper.Map<ProductViewModel>(product));
      }

      var sellerViewModel = _mapper.Map<GetSellerProductsByIdentityIdViewModel>(seller);
      sellerViewModel.Products = productViewModels;

      return new PagedResponse<GetSellerProductsByIdentityIdViewModel>(sellerViewModel, validFilter.PageNumber, validFilter.PageSize, dataCount);
    }
  }
}