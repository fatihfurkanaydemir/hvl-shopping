using Common.EventBus.RPCs;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class CanUseCouponRPC: RPC<Response<bool>>
{
  public int DiscountId { get; set; }
  public string CustomerIdentityId { get; set; }
}
