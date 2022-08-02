using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.DeleteCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Queries.GetAllCategories;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.Categories.Queries.GetCategoryProductsById;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers.v1
{
  public class CategoryController : BaseApiController
  {
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // GET: api/<controller>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllCategoriesParameter filter)
    {
      return Ok(await Mediator.Send(new GetAllCategoriesQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
    }

    // GET: api/<controller>/id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      return Ok(await Mediator.Send(new GetCategoryByIdQuery() { Id = id }));
    }

    // GET: api/<controller>/id/products
    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetCategoryProductsById(int id, [FromQuery] GetCategoryProductsByIdParameter filter)
    {
      return Ok(await Mediator.Send(new GetCategoryProductsByIdQuery() { Id = id, PageNumber = filter.PageNumber, PageSize = filter.PageSize }));
    }

    // PATCH: api/<controller>
    [HttpPatch]
    public async Task<IActionResult> Patch(UpdateCategoryCommand command)
    {
      return Ok(await Mediator.Send(command));
    }

    // DELETE: api/<controller>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      return Ok(await Mediator.Send(new DeleteCategoryCommand { Id = id }));
    }
  }
}
