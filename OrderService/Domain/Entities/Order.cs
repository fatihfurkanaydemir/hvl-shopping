using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Order: AuditableBaseEntity
{
  public string CustomerIdentityId { get; set; }
  public string SellerIdentityId { get; set; }
  public string ProductId { get; set; }
  public DateTime CreatedDate { get; set; }
  public OrderStatus Status { get; set; }
}
