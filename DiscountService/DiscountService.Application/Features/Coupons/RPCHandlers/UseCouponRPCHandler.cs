using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class UseCouponRPCHandler: IRPCHandler<UseCouponRPC, Response<decimal>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IUsedCouponRepositoryAsync _usedCouponRepository;
  public UseCouponRPCHandler(ICouponRepositoryAsync couponRepository, IUsedCouponRepositoryAsync usedCouponRepository)
  {
    _couponRepository = couponRepository;
    _usedCouponRepository = usedCouponRepository;
  }

  public async Task<Response<decimal>> Handle(UseCouponRPC rpc)
  {
    var coupon = await _couponRepository.GetByCodeAsync(rpc.CouponCode);
    if(coupon == null)
    {
      return new Response<decimal>
      {
        Succeeded = false,
        Message = "Coupon not found.",
      };
    }

    if (coupon.ExpireDate < DateTime.Now)
    {
      return new Response<decimal>
      {
        Succeeded = false,
        Message = "Coupon expired.",
      };
    }

    if (coupon.Status == Common.Enums.CouponStatus.Passive)
    {
      return new Response<decimal>
      {
        Succeeded = false,
        Message = "Coupon not active.",
      };
    }

    var didUseCoupon = await _usedCouponRepository.DidCustomerUseCoupon(rpc.CustomerIdentityId, coupon.Id);
    if(didUseCoupon)
    {
      return new Response<decimal>
      {
        Succeeded = false,
        Message = "This customer already used this coupon.",
      };
    }

    await _couponRepository.MarkUnchangedAsync(coupon);
    await _usedCouponRepository.AddAsync(new UsedCoupon { CustomerIdentityId = rpc.CustomerIdentityId, Coupon = coupon });

    return new Response<decimal>(coupon.Amount, "Coupon used");
  }
}
