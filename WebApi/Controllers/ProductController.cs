using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Commands.ActivateProduct;
using Application.Features.Products.Commands.DeactivateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers.v1
{
  public class ProductController : BaseApiController
  {
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
    {
      return Ok(await Mediator.Send(new GetAllProductsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/<controller>/id
    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      return Ok(await Mediator.Send(new GetProductByIdQuery() { Id = id }));
    }

    // PATCH: api/<controller>
    [HttpPatch]
    public async Task<IActionResult> Patch(UpdateProductCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // POST: api/<controller>/deactivate
    [HttpPost("deactivate")]
    public async Task<IActionResult> Deactivate(DeactivateProductCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // POST: api/<controller>/activate
    [HttpPost("activate")]
    public async Task<IActionResult> Activate(ActivateProductCommand command)
    {
      return Ok(await Mediator.Send(command));
    }
  }
}
