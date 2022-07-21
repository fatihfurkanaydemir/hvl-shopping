using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Products.Queries.GetAllProducts
{
  public class GetAllProductsViewModel
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public ProductStatus Status { get; set; }
    public Category Category { get; set; }
  }
}
