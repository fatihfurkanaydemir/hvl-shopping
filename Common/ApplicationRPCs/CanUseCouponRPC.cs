using Common.EventBus.RPCs;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class CanUseCouponRPC: RPC<Response<bool>>
{
  public string CouponCode { get; set; }
  public string CustomerIdentityId { get; set; }
}
