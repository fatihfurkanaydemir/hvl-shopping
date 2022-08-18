using Common.Entities;
using Common.Enums;

namespace DiscountService.Domain.Entities;

public class UsedCoupon: AuditableBaseEntity
{
  public string CustomerIdentityId { get; set; }
  public Coupon Coupon { get; set; }
}
