using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using DataAnnotationsExtensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
  public class CreateProductCommand : IRequest<Response<int>>
  {
    public string SellerIdentityId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public List<ImageDTO> Images { get; set; }
    [Min(1)]
    public decimal Price { get; set; }
    public int InStock { get; set; }
  }
  public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<int>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public CreateProductCommandHandler(IProductRepositoryAsync productRepository, ICategoryRepositoryAsync categoryRepository, ISellerRepositoryAsync sellerRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _sellerRepository = sellerRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
      var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
      if (category == null) throw new ApiException("Category not found");

      var seller = await _sellerRepository.GetByIdentityIdAsync(request.SellerIdentityId);
      if (seller == null) throw new ApiException("Seller not found");

      var product = _mapper.Map<Product>(request);

      await _categoryRepository.MarkUnchangedAsync(category);
      product.Category = category;

      await _sellerRepository.MarkUnchangedAsync(seller);
      product.Seller = seller;

      product.Status = ProductStatus.Active;
      product.Sold = 0;

      await _productRepository.AddAsync(product);

      return new Response<int>(product.Id);
    }
  }
}
