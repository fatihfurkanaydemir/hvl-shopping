using Common.Entities;
using Common.Enums;

namespace DiscountService.Domain.Entities;

public class UsedDiscount: AuditableBaseEntity
{
  public string CustomerIdentityId { get; set; }
  public Discount Discount { get; set; }
}
