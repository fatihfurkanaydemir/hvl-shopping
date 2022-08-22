using Common.Entities;
using Common.Enums;

namespace DiscountService.Domain.Entities;

public class Coupon: AuditableBaseEntity
{
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpireDate { get; set; }
  public CouponStatus Status { get; set; }
}
