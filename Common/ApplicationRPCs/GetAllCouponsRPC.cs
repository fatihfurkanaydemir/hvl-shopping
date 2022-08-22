using Common.EventBus.RPCs;
using Common.SharedViewModels;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class GetAllCouponsRPC: RPC<PagedResponse<IEnumerable<CouponViewModel>>>
{
  public int PageSize { get; set; }
  public int PageNumber { get; set; }
}
