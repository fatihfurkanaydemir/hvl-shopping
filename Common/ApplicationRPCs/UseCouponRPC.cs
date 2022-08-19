using Common.EventBus.RPCs;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class UseCouponRPC: RPC<Response<decimal>>
{
  public string CouponCode { get; set; }
  public string CustomerIdentityId { get; set; }
}
