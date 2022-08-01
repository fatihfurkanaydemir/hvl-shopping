using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Categories.Queries.GetCategoryProductsById
{
  public class GetCategoryProductsByIdProductViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public List<GetCategoryProductsByIdImageViewModel> Images { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public string Status { get; set; }
  }
}
