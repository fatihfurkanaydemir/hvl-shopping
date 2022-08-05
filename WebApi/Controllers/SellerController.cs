using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
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

    //// GET: api/<controller>/id
    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetById(int id)
    //{
    //  return Ok(await Mediator.Send(new GetCustomerByIdQuery() { Id = id }));
    //}

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
