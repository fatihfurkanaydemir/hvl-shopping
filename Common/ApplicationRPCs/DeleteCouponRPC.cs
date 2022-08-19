using Common.EventBus.RPCs;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class DeleteCouponRPC: RPC<Response<int>>
{
  public string Code { get; set; }
}
