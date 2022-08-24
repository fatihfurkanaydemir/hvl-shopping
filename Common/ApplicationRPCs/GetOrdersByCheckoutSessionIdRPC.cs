using Common.EventBus.RPCs;
using Common.SharedViewModels;
using Common.Wrappers;

namespace Common.ApplicationRPCs;

public class GetOrdersByCheckoutSessionIdRPC : RPC<Response<IEnumerable<OrderViewModel>>>
{
  public string CheckoutSessionId { get; set; }
}
