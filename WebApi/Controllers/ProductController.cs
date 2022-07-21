using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;


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

    // GET: api/<controller>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      return Ok(await Mediator.Send(new GetProductByIdQuery() { Id = id }));
    }
  }
}
