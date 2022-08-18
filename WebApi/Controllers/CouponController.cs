using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using WebApi.Application.Features.SharedViewModels;
using Application.Wrappers;
using Application.Parameters;
using Application.Exceptions;

namespace WebApi.Controllers.v1
{
  public class CouponController : BaseApiController
  {
    private readonly IEventBus _eventBus;
    public CouponController(IEventBus eventBus)
    {
      _eventBus = eventBus;
    }
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Post(CreateCouponRPC rpc)
    {
      var response = await _eventBus.CallRP(rpc);
      if (!response.Succeeded) throw new ApiException(response.Message);
      return Ok(response);
    }

    // GET api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] RequestParameter filter)
    {
      var response = await _eventBus.CallRP(new GetAllCouponsRPC { PageNumber = filter.PageNumber, PageSize = filter.PageSize });
      if(!response.Succeeded) throw new ApiException(response.Message);
      return Ok(response);
    }

    // GET api/<controller>/CustomerUsedCoupons/customerIdentityId
    [HttpGet("CustomerUsedCoupons/{customerIdentityId}")]
    public async Task<IActionResult> GetCustomerUsedCoupons(string customerIdentityId, [FromQuery] RequestParameter filter)
    {
      var response = await _eventBus.CallRP(new GetUsedCouponsByCustomerIdentityIdRPC { CustomerIdentityId = customerIdentityId, PageNumber = filter.PageNumber, PageSize = filter.PageSize });
      if (!response.Succeeded) throw new ApiException(response.Message);
      return Ok(response);
    }

    // POST api/<controller>/CanUseCoupon
    [HttpPost("CanUseCoupon")]
    public async Task<IActionResult> CanUseCoupon(CanUseCouponRPC rpc)
    {
      var response = await _eventBus.CallRP(rpc);
      if (!response.Succeeded) throw new ApiException(response.Message);
      return Ok(response);
    }

    // POST api/<controller>/UseCoupon
    [HttpPost("UseCoupon/")]
    public async Task<IActionResult> UseCoupon(UseCouponRPC rpc)
    {
      var response = await _eventBus.CallRP(rpc);
      if (!response.Succeeded) throw new ApiException(response.Message);
      return Ok(response);
    }

  }
}
