using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Commands.DeactivateProduct
{

  public class DeactivateProductCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class DeleteProductCommandHandler : IRequestHandler<DeactivateProductCommand, Response<int>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IMapper _mapper;
    public DeleteProductCommandHandler(IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdAsync(request.Id);
      if (product == null) throw new ApiException("Product not found");

      if (product.Status == ProductStatus.Active)
      {
        product.Status = ProductStatus.Passive;
        await _productRepository.UpdateAsync(product);
      }

      return new Response<int>(product.Id);
    }
  }
}
