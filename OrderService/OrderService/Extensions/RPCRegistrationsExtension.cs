using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using OrderService.Application.Features.Orders.RPCHandlers;
using Common.Wrappers;
using Common.SharedViewModels;

namespace OrderService.Extensions;

public static class RPCSubscriptionsExtension
{
  public static void RegisterRPCs(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.RegisterRPCHandler<GetOrdersByCheckoutSessionIdRPC, GetOrdersByCheckoutSessionIdRPCHandler, Response<IEnumerable<OrderViewModel>>>();

  }
}
