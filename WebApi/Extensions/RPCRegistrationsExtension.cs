using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using Common.Wrappers;
using Common.SharedViewModels;

namespace WebApi.Extensions;

public static class RPCSubscriptionsExtension
{
  public static void RegisterRPCs(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.RegisterRPC<TestRPC, string>();

    eventBus.RegisterRPC<CanUseCouponRPC, Response<bool>>();
    eventBus.RegisterRPC<UseCouponRPC, Response<decimal>>();
    eventBus.RegisterRPC<CreateCouponRPC, Response<int>>();
    eventBus.RegisterRPC<GetAllCouponsRPC, PagedResponse<IEnumerable<CouponViewModel>>>();
    eventBus.RegisterRPC<GetUsedCouponsByCustomerIdentityIdRPC, PagedResponse<IEnumerable<CouponViewModel>>>();
    eventBus.RegisterRPC<GetUsableCouponsByCustomerIdentityIdRPC, Response<IEnumerable<CouponViewModel>>>();
    eventBus.RegisterRPC<UpdateCouponRPC, Response<int>>();
  }
}
