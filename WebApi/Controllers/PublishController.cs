using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Commands.UpdateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
using Application.Features.Sellers.Queries.GetSellerProductsByIdentityId;
using Application.Features.Sellers.Queries.GetSellerByIdentityId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;

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
  }
}
