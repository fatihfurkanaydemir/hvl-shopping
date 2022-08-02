using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryProductsById
{
  public class GetCategoryProductsByIdQuery : IRequest<PagedResponse<GetCategoryProductsByIdViewModel>>
  {
    public int Id { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetCategoryProductsByIdQueryHandler : IRequestHandler<GetCategoryProductsByIdQuery, PagedResponse<GetCategoryProductsByIdViewModel>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public GetCategoryProductsByIdQueryHandler(ICategoryRepositoryAsync categoryRepository, IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<GetCategoryProductsByIdViewModel>> Handle(GetCategoryProductsByIdQuery request, CancellationToken cancellationToken)
    {
      var category = await _categoryRepository.GetByIdAsync(request.Id);
      if (category == null) throw new ApiException("Category not found");

      var validFilter = _mapper.Map<GetCategoryProductsByIdParameter>(request);
      var dataCount = await _productRepository.GetDataCountByCategoryIdAsync(request.Id);

      var products = await _productRepository.GetByCategoryIdWithRelationsAsync(request.Id, validFilter.PageNumber, validFilter.PageSize);

      var productViewModels = new List<GetCategoryProductsByIdProductViewModel>();
      foreach(Product product in products)
      {
        productViewModels.Add(_mapper.Map<GetCategoryProductsByIdProductViewModel>(product));
      }

      var categoryViewModel = _mapper.Map<GetCategoryProductsByIdViewModel>(category);
      categoryViewModel.Products = productViewModels;

      return new PagedResponse<GetCategoryProductsByIdViewModel>(categoryViewModel, validFilter.PageNumber, validFilter.PageSize, dataCount);
    }
  }
}
