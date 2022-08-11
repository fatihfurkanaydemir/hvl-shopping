using OrderService.Domain.Common;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class Order: AuditableBaseEntity
{
  public string CustomerIdentityId { get; set; }
  public string SellerIdentityId { get; set; }
  public string ProductId { get; set; }
  public DateTime CreatedDate { get; set; }
  public OrderStatus Status { get; set; }
}
