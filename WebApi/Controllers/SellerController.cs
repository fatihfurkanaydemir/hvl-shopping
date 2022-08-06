using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
using Application.Features.Sellers.Queries.GetSellerProductsByIdentityId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.v1
{
  public class SellerController : BaseApiController
  {
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Create(CreateSellerCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllSellersParameter filter)
    {
      return Ok(await Mediator.Send(new GetAllSellersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/<controller>/id/products
    [HttpGet("{identityId}/Products")]
    public async Task<IActionResult> GetProductsByIdentityId(string identityId, [FromQuery] GetSellerProductsByIdentityIdParameter filter)
    {
      return Ok(await Mediator.Send(new GetSellerProductsByIdentityIdQuery() { IdentityId = identityId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    //// PATCH: api/<controller>
    //[HttpPatch]
    //public async Task<IActionResult> Patch(UpdateCustomerCommand command)
    //{
    //  return Ok(await Mediator.Send(command));
    //}

    //// POST: api/<controller>/deactivate
    //[HttpPost("deactivate")]
    //public async Task<IActionResult> Deactivate(DeactivateCustomerCommand command)
    //{
    //  return Ok(await Mediator.Send(command));
    //}

    //// POST: api/<controller>/activate
    //[HttpPost("activate")]
    //public async Task<IActionResult> Activate(ActivateCustomerCommand command)
    //{
    //  return Ok(await Mediator.Send(command));
    //}
  }
}
