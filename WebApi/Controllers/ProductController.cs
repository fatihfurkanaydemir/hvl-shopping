using Application.Features.Products.Queries.GetAllProducts;


using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers.v1
{
  public class ProductController : BaseApiController
  {
    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
    {
      return Ok(await Mediator.Send(new GetAllProductsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }
  }
}
