using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers.v1;

public class NotificationController : BaseApiController
{

  //// GET: api/<controller>
  //[HttpGet]
  //public async Task<IActionResult> Get([FromQuery] GetAllOrdersParameter filter)
  //{
  //  return Ok(await Mediator.Send(new GetAllOrdersQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
  //}

  //// GET: api/<controller>/SellerOrders/identityId
  //[HttpGet("SellerOrders/{identityId}")]
  //public async Task<IActionResult> GetSellerOrders(string identityId, [FromQuery] GetAllOrdersParameter filter)
  //{
  //  return Ok(await Mediator.Send(new GetAllOrdersBySellerIdentityIdQuery() { IdentityId = identityId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
  //}

  //// GET: api/<controller>/CustomerOrders/identityId
  //[HttpGet("CustomerOrders/{identityId}")]
  //public async Task<IActionResult> GetCustomerOrders(string identityId, [FromQuery] GetAllOrdersParameter filter)
  //{
  //  return Ok(await Mediator.Send(new GetAllOrdersByCustomerIdentityIdQuery() { IdentityId = identityId, PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
  //}
}
