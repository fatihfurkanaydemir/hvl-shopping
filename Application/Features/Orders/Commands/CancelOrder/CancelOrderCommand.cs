using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Common.ApplicationEvents;
using Common.EventBus.Interfaces;
using Common.SharedViewModels;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Orders.Commands.CreateOrder
{
  public class CancelOrderCommand : IRequest<Response<string>>
  {
    [Required]
    public List<OrderProductViewModel> Products { get; set; }
    public int OrderId { get; set; }
  }
  public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Response<string>>
  {

    private readonly IProductRepositoryAsync _productRepository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    public CancelOrderCommandHandler(
      IProductRepositoryAsync productRepository,
      IEventBus eventBus,
      IMapper mapper
      )
    {
      _productRepository = productRepository;
      _eventBus = eventBus;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
      foreach(var productviewModel in request.Products)
      {
        var product = await _productRepository.GetByIdAsync(productviewModel.ProductId);
        await _productRepository.MarkUnchangedAsync(product);
        product.InStock += productviewModel.Count;

        await _productRepository.UpdateAsync(product);
      }

      _eventBus.Publish(new CancelOrderEvent
      {
        OrderId = request.OrderId
      });

      return new Response<string>(request.Products.Count().ToString(), "Products restocked, order canceled");
    }
  }
}
