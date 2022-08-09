using Application.Features.Customers.Commands.CreateCustomer;
using Application.Features.Customers.Commands.UpdateCustomer;
using Application.Features.Customers.Commands.DeleteAddress;
using Application.Features.Customers.Commands.UpdateAddress;
using Application.Features.Customers.Queries.GetAllCustomers;
using Application.Features.Customers.Queries.GetCustomerByIdentityId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Customers.Commands.AddAddress;

namespace WebApi.Controllers.v1
{
  public class CustomerController : BaseApiController
  {
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllCustomersParameter filter)
    {
      return Ok(await Mediator.Send(new GetAllCustomersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/<controller>/id
    [HttpGet("{identityId}")]
    public async Task<IActionResult> GetCustomerByIdentityId(string identityId)
    {
      return Ok(await Mediator.Send(new GetCustomerByIdentityIdQuery() { identityId = identityId }));
    }

    // PATCH: api/<controller>
    [HttpPatch]
    public async Task<IActionResult> UpdateCustomer(UpdateCustomerCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // PATCH: api/<controller>
    [HttpPatch("UpdateAddress")]
    public async Task<IActionResult> UpdateAddress(UpdateAddressCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // POST api/<controller>/AddAddress
    [HttpPost("AddAddress")]
    public async Task<IActionResult> AddAddress(AddAddressCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // POST api/<controller>/DeleteAddress
    [HttpDelete("DeleteAddress")]
    public async Task<IActionResult> DeleteAddress(DeleteAddressCommand command)
    {
      return Ok(await Mediator.Send(command));
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
