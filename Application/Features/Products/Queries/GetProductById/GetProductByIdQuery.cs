using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById
{
  public class GetProductByIdQuery : IRequest<Response<GetProductByIdViewModel>>
  {
    public int Id { get; set; }
  }

  public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<GetProductByIdViewModel>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IMapper _mapper;
    public GetProductByIdQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<Response<GetProductByIdViewModel>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
      var product = await _productRepository.GetByIdWithRelationsAsync(request.Id);
      var productViewModel = _mapper.Map<GetProductByIdViewModel>(product);


      return new Response<GetProductByIdViewModel>(productViewModel);
    }
  }
}
