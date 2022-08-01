using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Enums;
using MediatR;

namespace Application.Features.Products.Commands.ActivateProduct
{

  public class ActivateProductCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
  }
  public class DeleteProductCommandHandler : IRequestHandler<ActivateProductCommand, Response<int>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IMapper _mapper;
    public DeleteProductCommandHandler(IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdAsync(request.Id);
      if (product == null) throw new ApiException("Product not found");

      if(product.Status == ProductStatus.Passive)
      {
        product.Status = ProductStatus.Active;
        await _productRepository.UpdateAsync(product);
      }

      return new Response<int>(product.Id);
    }
  }
}
