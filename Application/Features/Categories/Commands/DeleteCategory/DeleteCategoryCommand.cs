using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.DeleteCategory
{
  public class DeleteCategoryCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<int>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public DeleteCategoryCommandHandler(ICategoryRepositoryAsync categoryRepository, IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
      var category = await _categoryRepository.GetByIdWithRelationsAsync(request.Id);
      if (category == null) throw new ApiException("Category not found");

      var othersCategory = await _categoryRepository.GetByNameAsync("Diğer");
      if (othersCategory == null) throw new ApiException("Others category not found");

      if(category.Id == othersCategory.Id) throw new ApiException("Others category can not be deleted");

      for(int i = 0; i < category.Products.Count; ++i)
      {
        Product product = category.Products.ElementAt(i);
        product.Category = othersCategory;
        await _productRepository.UpdateAsync(product);
        othersCategory.Products.Add(product);
      }

      category.Products.Clear();
      await _categoryRepository.DeleteAsync(category);
      await _categoryRepository.UpdateAsync(othersCategory);

      return new Response<int>(category.Id);
    }
  }
}
