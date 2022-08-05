using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Product: AuditableBaseEntity
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public List<Image> Images { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public Category Category { get; set; }
    public Seller Seller { get; set; }
  }
}
