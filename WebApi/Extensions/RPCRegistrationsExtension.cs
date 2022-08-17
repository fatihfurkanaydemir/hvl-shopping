using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;

namespace WebApi.Extensions;

public static class RPCSubscriptionsExtension
{
  public static void RegisterRPCs(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.RegisterRPC<TestRPC, string>();
  }
}
