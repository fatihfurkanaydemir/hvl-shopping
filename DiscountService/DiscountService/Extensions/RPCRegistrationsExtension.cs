using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using DiscountService.Application.Features.Test;
using DiscountService.Application.Features.Coupons.RPCHandlers;
using Common.Wrappers;
using Common.SharedViewModels;

namespace DiscountService.Extensions;

public static class RPCSubscriptionsExtension
{
  public static void RegisterRPCs(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.RegisterRPCHandler<TestRPC, TestRPCHandler, string>();
    eventBus.RegisterRPCHandler<CanUseCouponRPC, CanUseCouponRPCHandler, Response<bool>>();
    eventBus.RegisterRPCHandler<UseCouponRPC, UseCouponRPCHandler, Response<bool>>();
    eventBus.RegisterRPCHandler<CreateCouponRPC, CreateCouponRPCHandler, Response<int>>();
    eventBus.RegisterRPCHandler<GetAllCouponsRPC, GetAllCouponsRPCHandler, PagedResponse<IEnumerable<CouponViewModel>>>();
    eventBus.RegisterRPCHandler<GetUsedCouponsByCustomerIdentityIdRPC, GetUsedCouponsByCustomerIdentityIdRPCHandler, PagedResponse<IEnumerable<CouponViewModel>>>();
  }
}
