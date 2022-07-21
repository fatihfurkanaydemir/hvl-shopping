using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
  // WARN
  // Temporary DTO class for testing. Will be removed
  public class ImageDTO
  {
    public string Url { get; set; }
  }

  public class CategoryDTO
  {
    public string Name { get; set; }
  }

  public class CreateProductCommand : IRequest<Response<int>>
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public List<ImageDTO> Images { get; set; }
    public int InStock { get; set; }
    public CategoryDTO Category { get; set; }
  }
  public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<int>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IMapper _mapper;
    public CreateProductCommandHandler(IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
      var product = _mapper.Map<Product>(request);
      product.Status = ProductStatus.Active;
      product.Sold = 0;

      await _productRepository.AddAsync(product);

      return new Response<int>(product.Id);
    }
  }
}
