using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class CanUseCouponRPCHandler: IRPCHandler<CanUseCouponRPC, Response<bool>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IUsedCouponRepositoryAsync _usedCouponRepository;
  public CanUseCouponRPCHandler(ICouponRepositoryAsync couponRepository, IUsedCouponRepositoryAsync usedCouponRepository)
  {
    _couponRepository = couponRepository;
    _usedCouponRepository = usedCouponRepository;
  }

  public async Task<Response<bool>> Handle(CanUseCouponRPC rpc)
  {
    var coupon = await _couponRepository.GetByCodeAsync(rpc.CouponCode);
    if(coupon == null)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "Coupon not found.",
      };
    }

    if (coupon.ExpireDate < DateTime.UtcNow)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "Coupon expired.",
      };
    }

    if (coupon.Status == Common.Enums.CouponStatus.Passive)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "Coupon not active.",
      };
    }

    var didUseCoupon = await _usedCouponRepository.DidCustomerUseCoupon(rpc.CustomerIdentityId, coupon.Id);
    if(didUseCoupon)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "This customer already used this coupon.",
      };
    }

    return new Response<bool>(true, "Coupon is usable");
  }
}
