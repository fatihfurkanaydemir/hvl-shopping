using Microsoft.AspNetCore.Mvc;
using Common.EventBus.Interfaces;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Commands.CreateCheckoutSession;

namespace WebApi.Controllers.v1
{
  public class OrderController : BaseApiController
  {
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // POST api/<controller>
    [HttpPost("checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession(CreateCheckoutSessionCommand command)
    {
      return Ok(await Mediator.Send(command));
    }
  }
}
