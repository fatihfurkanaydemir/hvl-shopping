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
using Domain.Entities;
using Application.Services;
using Common.ApplicationRPCs;

namespace Application.Features.Orders.Commands.CreateOrder
{
  public class CreateOrderCommand : IRequest<Response<string>>
  {
    [Required]
    public string CustomerIdentityId { get; set; }
    [Required]
    public decimal ShipmentPrice { get; set; }
    [Required]
    public string AddressTitle { get; set; }
    [Required]
    public string AddressDescription { get; set; }
    [Required]
    public string AddressCity { get; set; }
    public string? CouponCode { get; set; }
  }
  public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<string>>
  {
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IBasketRepositoryAsync _basketRepository;
    private readonly PaymentService _paymentService;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    public CreateOrderCommandHandler(
      ISellerRepositoryAsync sellerRepository,
      ICustomerRepositoryAsync customerRepository,
      IProductRepositoryAsync productRepository,
      IBasketRepositoryAsync basketRepository,
      PaymentService paymentService,
      IEventBus eventBus,
      IMapper mapper
      )
    {
      _sellerRepository = sellerRepository;
      _customerRepository = customerRepository;
      _productRepository = productRepository;
      _basketRepository = basketRepository;
      _paymentService = paymentService;
      _eventBus = eventBus;
      _mapper = mapper;
    }

    public async Task<Response<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(request.CustomerIdentityId);
      if (customer == null) throw new ApiException("Customer not found");

      var basket = await _basketRepository.GetBasketAsync(request.CustomerIdentityId);
      if (basket == null) throw new ApiException("Basket not found");
      if (basket.Items.Count <= 0) throw new ApiException("No products in basket");

      var sellerProductMap = await GetSellerProductMap(basket.Items);

      var orderGroupId = Guid.NewGuid().ToString() + customer.IdentityId;
      var orderEvents = new List<CreateOrderEvent>();

      foreach(var sellerIdentityId in sellerProductMap.Keys)
      {
        var seller = await _sellerRepository.GetByIdentityIdAsync(sellerIdentityId);
        if (seller == null) throw new ApiException("Seller not found");

        var orderEvent = new CreateOrderEvent
        {
          AddressCity = request.AddressCity,
          AddressDescription = request.AddressDescription,
          AddressTitle = request.AddressTitle,
          CustomerFirstName = customer.FirstName,
          CustomerLastName = customer.LastName,
          CustomerIdentityId = customer.IdentityId,
          CustomerPhoneNumber = customer.PhoneNumber,
          SellerIdentityId = sellerIdentityId,
          Status = OrderStatus.AwaitingPayment,
          ShipmentPrice = request.ShipmentPrice,
          OrderGroupId = orderGroupId,
          Products = new List<OrderProduct>()
        };

        foreach (var p in sellerProductMap[sellerIdentityId])
        {
          if (p.Quantity <= 0) throw new ApiException("Invalid product count");

          var product = await _productRepository.GetByIdWithRelationsAsync(p.Id);
          if (product == null) throw new ApiException($"Product ({p.ProductName}) not found");

          if (product.Seller.Id != seller.Id) throw new ApiException($"Seller does not have the product ({product.Name})");
          if (product.InStock < p.Quantity) throw new ApiException($"Not enough stock for the product ({product.Name})");
          if (product.Status == Domain.Enums.ProductStatus.Passive) throw new ApiException($"Product is not active ({product.Name})");

          orderEvent.Products.Add(new OrderProduct
          {
            ProductId = product.Id,
            ProductName = product.Name,
            Count = p.Quantity,
            PricePerProduct = product.Price
          });

          orderEvent.TotalProductPrice += p.Quantity * product.Price;
        }

        orderEvents.Add(orderEvent);
      }

      Stripe.Checkout.Session session;

      // [TODO] needs refactoring 
      if (request.CouponCode != null && request.CouponCode != "")
      {
        var response = await _eventBus.CallRP(new UseCouponRPC { CustomerIdentityId = request.CustomerIdentityId, CouponCode = request.CouponCode });
        if (!response.Succeeded) throw new ApiException(response.Message);
        var couponAmount = response.Data;

        session = await _paymentService.CreateCheckoutSession(basket, request.ShipmentPrice, request.CouponCode, couponAmount);
        if (session.AmountTotal <= couponAmount)
        {
          throw new ApiException("Coupon amount can not be more than total");
        }

        foreach (var orderEvent in orderEvents)
        {
          orderEvent.CheckoutSessionId = session.Id;
          orderEvent.CheckoutSessionUrl = session.Url;
          orderEvent.PaymentIntentId = "";
          orderEvent.CouponCode = request.CouponCode;
          orderEvent.CouponAmount = couponAmount;

          _eventBus.Publish(orderEvent);
        }
      }
      else
      {
        session = await _paymentService.CreateCheckoutSession(basket, request.ShipmentPrice);

        foreach (var orderEvent in orderEvents)
        {
          orderEvent.CheckoutSessionId = session.Id;
          orderEvent.CheckoutSessionUrl = session.Url;
          orderEvent.PaymentIntentId = "";
          orderEvent.CouponCode = "";
          orderEvent.CouponAmount = 0;

          _eventBus.Publish(orderEvent);
        }
      }

      foreach (var sellerIdentityId in sellerProductMap.Keys)
      {
        foreach (var p in sellerProductMap[sellerIdentityId])
        {
          var product = await _productRepository.GetByIdWithRelationsAsync(p.Id);
          product.InStock -= p.Quantity;
          product.Sold += p.Quantity;
          await _productRepository.UpdateAsync(product);
        }
      }

      return new Response<string>(session.Url, "Order and checkout session created");
    }

    public async Task<Dictionary<string, List<BasketItem>>> GetSellerProductMap(List<BasketItem> items)
    {
      var map = new Dictionary<string, List<BasketItem>>();
      
      foreach(var item in items)
      {
        if(map.ContainsKey(item.SellerIdentityId))
        {
          map[item.SellerIdentityId].Add(item);
          continue;
        }

        map[item.SellerIdentityId] = new List<BasketItem>
        {
          item
        };
      }

      return map;
    } 
  }
}
