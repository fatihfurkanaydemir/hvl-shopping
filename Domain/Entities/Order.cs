using Domain.Common;

namespace Domain.Entities
{
  public class Order: AuditableBaseEntity
  {
    public Product Product { get; set; }
    public Seller Seller { get; set; }
    public Customer Customer { get; set; }
  }
}
