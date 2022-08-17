using Common.Entities;
using DiscountService.Domain.Enums;

namespace DiscountService.Domain.Entities;

public class Discount: AuditableBaseEntity
{
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpireDate { get; set; }
  public DiscountStatus Status { get; set; }
}
