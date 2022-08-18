using Common.Enums;

namespace DiscountService.Application.Features.SharedViewModels;

public class CouponViewModel
{
  public int Id { get; set; }
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpireDate { get; set; }
  public CouponStatus Status { get; set; }
}
