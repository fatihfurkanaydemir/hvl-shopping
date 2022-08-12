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

namespace Application.Features.Orders.Commands.CreateOrder
{
  public class CreateOrderCommand : IRequest<Response<int>>
  {
    [Required]
    public string CustomerIdentityId { get; set; }
    [Required]
    public string SellerIdentityId { get; set; }
    [Required]
    public List<OrderProductDTO> Products { get; set; }
    [Required]
    public string AddressTitle { get; set; }
    [Required]
    public string AddressDescription { get; set; }
    [Required]
    public string AddressCity { get; set; }
  }
  public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<int>>
  {
    private readonly ISellerRepositoryAsync _sellerRepository;
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IProductRepositoryAsync _productRepository;
    private readonly IEventBus _eventBus;
    private readonly IMapper _mapper;
    public CreateOrderCommandHandler(
      ISellerRepositoryAsync sellerRepository,
      ICustomerRepositoryAsync customerRepository,
      IProductRepositoryAsync productRepository,
      IEventBus eventBus,
      IMapper mapper
      )
    {
      _sellerRepository = sellerRepository;
      _customerRepository = customerRepository;
      _productRepository = productRepository;
      _eventBus = eventBus;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
      if (request.Products.Count <= 0) throw new ApiException("No products provided");

      var orderEvent = _mapper.Map<CreateOrderEvent>(request);

      var seller = await _sellerRepository.GetByIdentityIdAsync(orderEvent.SellerIdentityId);
      if (seller == null) throw new ApiException("Seller not found");

      var customer = await _customerRepository.GetByIdentityIdWithRelationsAsync(orderEvent.CustomerIdentityId);
      if (customer == null) throw new ApiException("Customer not found");

      orderEvent.Products.Clear();

      foreach(var p in request.Products)
      {
        if(p.Count <= 0) throw new ApiException("Invalid product count");

        var product = await _productRepository.GetByIdWithRelationsAsync(p.ProductId);
        if (product == null) throw new ApiException($"Product ({p.ProductId}) not found");

        if(product.Seller.Id != seller.Id) throw new ApiException($"Seller does not have the product ({product.Name})");
        if(product.InStock < p.Count) throw new ApiException($"Not enough stock for the product ({product.Name})");
        if(product.Status == Domain.Enums.ProductStatus.Passive) throw new ApiException($"Product is not active ({product.Name})");

        orderEvent.Products.Add(new OrderProduct
        {
          ProductId = product.Id,
          ProductName = product.Name,
          Count = p.Count,
          PricePerProduct = product.Price
        });

        orderEvent.TotalPrice += p.Count * product.Price;
      }

      orderEvent.CustomerFirstName = customer.FirstName;
      orderEvent.CustomerLastName = customer.LastName;
      orderEvent.CustomerPhoneNumber = customer.PhoneNumber;

      _eventBus.Publish(orderEvent);

      foreach (var p in request.Products)
      {
        var product = await _productRepository.GetByIdWithRelationsAsync(p.ProductId);
        product.InStock -= p.Count;
        product.Sold += p.Count;
        await _productRepository.UpdateAsync(product);
      }

      return new Response<int>("Order created");
    }
  }
}
