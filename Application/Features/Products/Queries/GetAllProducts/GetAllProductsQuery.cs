using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Products.Queries.GetAllProducts
{
  public class GetAllProductsQuery : IRequest<PagedResponse<IEnumerable<GetAllProductsViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<IEnumerable<GetAllProductsViewModel>>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IMapper _mapper;
    public GetAllProductsQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllProductsViewModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllProductsParameter>(request);
      var products = await _productRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize);

      var productViewModels = new List<GetAllProductsViewModel>();

      foreach (var p in products)
      {
        var community = _mapper.Map<GetAllProductsViewModel>(p);
        productViewModels.Add(community);
      }

      return new PagedResponse<IEnumerable<GetAllProductsViewModel>>(productViewModels, validFilter.PageNumber, validFilter.PageSize);
    }
  }
}
