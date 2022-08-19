using Common.EventBus.RPCs;
using Common.SharedViewModels;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class GetUsableCouponsByCustomerIdentityIdRPC: RPC<Response<IEnumerable<CouponViewModel>>>
{
  public string CustomerIdentityId { get; set; }
}
