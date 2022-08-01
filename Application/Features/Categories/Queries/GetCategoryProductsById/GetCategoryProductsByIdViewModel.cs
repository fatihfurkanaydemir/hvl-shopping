using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Categories.Queries.GetCategoryProductsById
{
  public class GetCategoryProductsByIdViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<GetCategoryProductsByIdProductViewModel> Products { get; set; }
  }
}
