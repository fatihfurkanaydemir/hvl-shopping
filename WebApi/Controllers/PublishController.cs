using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Commands.UpdateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
using Application.Features.Sellers.Queries.GetSellerProductsByIdentityId;
using Application.Features.Sellers.Queries.GetSellerByIdentityId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using Application.Features.Orders.Commands.CreateOrder;

namespace WebApi.Controllers.v1
{
  public class PublishController : BaseApiController
  {
    private readonly IEventBus _eventBus;
    public PublishController(IEventBus eventBus)
    {
      _eventBus = eventBus;
    }
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Publish(string message)
    {
      _eventBus.Publish<TestEvent>(new TestEvent { message = "Test event" });
      return Ok();
    }

    // POST api/<controller>/TestRPC
    [HttpPost("TestRPC")]
    public async Task<IActionResult> TestRPC(string message)
    {
      return Ok(await _eventBus.CallRP(new TestRPC { message = "Test RPC" }));
    }

    // POST api/<controller>/TestRPC
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(string message)
    {
      var orders = (await _eventBus.CallRP(new GetOrdersByCheckoutSessionIdRPC
      {
        CheckoutSessionId = message,
      })).Data;

      foreach (var order in orders)
      {
        await Mediator.Send(new CancelOrderCommand { Products = order.Products, OrderId = order.Id });
      }

      return Ok(orders.Count());
    }

  }
}
