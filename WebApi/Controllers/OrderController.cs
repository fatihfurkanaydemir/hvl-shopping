using Microsoft.AspNetCore.Mvc;
using Common.EventBus.Interfaces;
using Application.Features.Orders.Commands.CreateOrder;

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
  }
}
