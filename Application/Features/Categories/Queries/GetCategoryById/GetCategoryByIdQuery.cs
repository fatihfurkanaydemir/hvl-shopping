using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById
{
  public class GetCategoryByIdQuery : IRequest<Response<GetCategoryByIdViewModel>>
  {
    public int Id { get; set; }
  }

  public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Response<GetCategoryByIdViewModel>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public GetCategoryByIdQueryHandler(ICategoryRepositoryAsync categoryRepository, IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<Response<GetCategoryByIdViewModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
      var category = await _categoryRepository.GetByIdAsync(request.Id);
      if(category == null) throw new ApiException($"Category Not Found.");

      var productCount = await _productRepository.GetDataCountByCategoryIdAsync(category.Id);

      var categoryViewModel = _mapper.Map<GetCategoryByIdViewModel>(category);
      categoryViewModel.ProductCount = productCount;

      return new Response<GetCategoryByIdViewModel>(categoryViewModel);
    }
  }
}
