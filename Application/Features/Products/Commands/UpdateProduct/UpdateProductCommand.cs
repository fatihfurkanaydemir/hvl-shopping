using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
  // WARN
  // Temporary DTO class for testing. Will be removed
  public class ImageDTO
  {
    public string Url { get; set; }
  }

  public class UpdateProductCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public List<ImageDTO>? Images { get; set; }
    public int InStock { get; set; }
    public int CategoryId { get; set; }
  }
  public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<int>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public UpdateProductCommandHandler(IProductRepositoryAsync productRepository, ICategoryRepositoryAsync categoryRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdWithRelationsAsync(request.Id);
      if (product == null) throw new ApiException("Product not found");

      var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
      if (category == null) throw new ApiException("Category not found");

      var requestProduct = _mapper.Map<Product>(request);

      product.Name = requestProduct.Name;
      product.Code = requestProduct.Code;
      product.Description = requestProduct.Description;
      product.Images = requestProduct.Images;
      product.InStock = requestProduct.InStock;

      await _categoryRepository.MarkUnchangedAsync(category);
      product.Category = category;

      await _productRepository.UpdateAsync(product);

      return new Response<int>(product.Id);
    }
  }
}
