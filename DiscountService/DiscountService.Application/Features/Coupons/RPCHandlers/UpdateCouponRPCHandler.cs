using AutoMapper;
using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class UpdateCouponRPCHandler : IRPCHandler<UpdateCouponRPC, Response<int>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IMapper _mapper;
  public UpdateCouponRPCHandler(ICouponRepositoryAsync couponRepository, IMapper mapper)
  {
    _couponRepository = couponRepository;
    _mapper = mapper;
  }

  public async Task<Response<int>> Handle(UpdateCouponRPC rpc)
  {
    var coupon = await _couponRepository.GetByCodeAsync(rpc.Code);
    if(coupon == null)
    {
      return new Response<int>
      {
        Succeeded = false,
        Message = "Coupon not found"
      };
    }

    coupon.Amount = rpc.Amount;
    coupon.ExpireDate = rpc.ExpireDate;
    coupon.Status = rpc.Status;

    await _couponRepository.UpdateAsync(coupon);

    return new Response<int>(coupon.Id, "Coupon updated");
  }
}
