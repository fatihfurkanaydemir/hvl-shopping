using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class UseCouponRPCHandler: IRPCHandler<UseCouponRPC, Response<bool>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IUsedCouponRepositoryAsync _usedCouponRepository;
  public UseCouponRPCHandler(ICouponRepositoryAsync couponRepository, IUsedCouponRepositoryAsync usedCouponRepository)
  {
    _couponRepository = couponRepository;
    _usedCouponRepository = usedCouponRepository;
  }

  public async Task<Response<bool>> Handle(UseCouponRPC rpc)
  {
    var coupon = await _couponRepository.GetByIdAsync(rpc.DiscountId);
    if(coupon == null)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "Coupon not found.",
      };
    }

    if (coupon.ExpireDate < DateTime.Now)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "Coupon expired.",
      };
    }

    if (coupon.Status < Common.Enums.DiscountStatus.Passive)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "Coupon not active.",
      };
    }

    var didUseCoupon = await _usedCouponRepository.DidCustomerUseCoupon(rpc.CustomerIdentityId, rpc.DiscountId);
    if(didUseCoupon)
    {
      return new Response<bool>
      {
        Succeeded = false,
        Message = "This customer already used this coupon.",
      };
    }

    await _usedCouponRepository.AddAsync(new UsedCoupon { CustomerIdentityId = rpc.CustomerIdentityId, Coupon = coupon });

    return new Response<bool>(true, "Coupon used");
  }
}
