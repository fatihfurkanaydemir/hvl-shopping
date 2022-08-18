using Common.Enums;
using Common.EventBus.RPCs;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class UpdateCouponRPC: RPC<Response<int>>
{
  public string Code { get; set; }
  public decimal Amount { get; set; }
  public DateTime ExpireDate { get; set; }
  public CouponStatus Status { get; set; }
}
