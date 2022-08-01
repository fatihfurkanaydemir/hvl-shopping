using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Categories.Queries.GetAllCategories
{
  public class GetAllCategoriesQuery : IRequest<PagedResponse<IEnumerable<GetAllCategoriesViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResponse<IEnumerable<GetAllCategoriesViewModel>>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public GetAllCategoriesQueryHandler(ICategoryRepositoryAsync categoryRepository, IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllCategoriesViewModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllCategoriesParameter>(request);
      var dataCount = await _categoryRepository.GetDataCount();
      var Categories = await _categoryRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize);

      var categoryViewModels = new List<GetAllCategoriesViewModel>();

      foreach (var c in Categories)
      {
        var productCount = await _productRepository.GetDataCountByCategoryIdAsync(c.Id);
        var category = _mapper.Map<GetAllCategoriesViewModel>(c);
        category.ProductCount = productCount;

        categoryViewModels.Add(category);
      }

      return new PagedResponse<IEnumerable<GetAllCategoriesViewModel>>(categoryViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
    }
  }
}
