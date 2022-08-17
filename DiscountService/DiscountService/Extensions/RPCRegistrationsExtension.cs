using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using DiscountService.Application.Features.Test;

namespace DiscountService.Extensions;

public static class RPCSubscriptionsExtension
{
  public static void RegisterRPCs(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.RegisterRPCHandler<TestRPC, TestRPCHandler, string>();
  }
}
