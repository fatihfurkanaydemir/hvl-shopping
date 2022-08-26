using Application.Interfaces.Repositories;
using Application.Wrappers;
using Application.Features.SharedViewModels;
using AutoMapper;
using MediatR;

namespace Application.Features.Products.Queries.GetProductsBySearchFilter
{
  public class GetProductsBySearchFilterQuery : IRequest<PagedResponse<IEnumerable<ProductViewModel>>>
  {
    public string FilterString { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetProductsBySearchFilterQueryHandler : IRequestHandler<GetProductsBySearchFilterQuery, PagedResponse<IEnumerable<ProductViewModel>>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IMapper _mapper;
    public GetProductsBySearchFilterQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<ProductViewModel>>> Handle(GetProductsBySearchFilterQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetProductsBySearchFilterParameter>(request);
      var dataCount = await _productRepository.GetDataCountBySearchFilterAsync(validFilter.FilterString);
      var products = await _productRepository.GetBySearchFilterWithRelationsAsync(validFilter.FilterString, request.PageNumber, request.PageSize);

      var productViewModels = new List<ProductViewModel>();

      foreach (var p in products)
      {
        var product = _mapper.Map<ProductViewModel>(p);
        productViewModels.Add(product);
      }

      return new PagedResponse<IEnumerable<ProductViewModel>>(productViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
    }
  }
}
