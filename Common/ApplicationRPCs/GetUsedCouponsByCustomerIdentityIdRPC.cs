using Common.EventBus.RPCs;
using Common.SharedViewModels;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class GetUsedCouponsByCustomerIdentityIdRPC: RPC<PagedResponse<IEnumerable<CouponViewModel>>>
{
  public string CustomerIdentityId { get; set; }
  public int PageSize { get; set; }
  public int PageNumber { get; set; }
}
