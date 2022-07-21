using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Products.Queries.GetProductById
{
  public class GetProductByIdViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public List<GetProductByIdImageViewModel> Images { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public string Status { get; set; }
    public Category Category { get; set; }
  }
}
