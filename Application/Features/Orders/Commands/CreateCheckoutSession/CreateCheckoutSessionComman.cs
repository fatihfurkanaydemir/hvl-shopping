using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Common.EventBus.Interfaces;
using Common.Entities;
using Common.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Common.ApplicationEvents;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Orders.Commands.CreateCheckoutSession
{
  public class CreateCheckoutSessionCommand : IRequest<Response<string>>
  {
    [Required]
    public string BasketId { get; set; }
  }
  public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, Response<string>>
  {
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IBasketRepositoryAsync _basketRepository;
    private readonly PaymentService _paymentService;
    private readonly IMapper _mapper;
    public CreateCheckoutSessionCommandHandler(
      IProductRepositoryAsync productRepository,
      IBasketRepositoryAsync basketRepository,
      PaymentService paymentService,
      IMapper mapper
      )
    {
      _productRepository = productRepository;
      _basketRepository = basketRepository;
      _paymentService = paymentService;

      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
      var basket = await _basketRepository.GetBasketAsync(request.BasketId);
      if (basket == null) throw new ApiException("Basket not found");
      
      foreach (var p in basket.Items)
      {
        if (p.Quantity <= 0) throw new ApiException("Invalid product count");

        var product = await _productRepository.GetByIdWithRelationsAsync(p.Id);
        if (product == null) throw new ApiException($"Product ({p.ProductName}) not found");

        if (product.InStock < p.Quantity) throw new ApiException($"Not enough stock for the product ({product.Name})");
        if (product.Status == Domain.Enums.ProductStatus.Passive) throw new ApiException($"Product is not active ({product.Name})");

        p.Price = product.Price;
      }

      var sessionUrl = await _paymentService.CreateCheckoutSession(basket);

      return new Response<string>(sessionUrl, "Session created");
    }
  }
}
