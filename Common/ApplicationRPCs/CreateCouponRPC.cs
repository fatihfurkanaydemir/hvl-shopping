using Common.Enums;
using Common.EventBus.RPCs;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class CreateCouponRPC: RPC<Response<int>>
{
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpireDate { get; set; }
  public DiscountStatus Status { get; set; }
}
