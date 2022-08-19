using AutoMapper;
using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class DeleteCouponRPCHandler : IRPCHandler<DeleteCouponRPC, Response<int>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IMapper _mapper;
  public DeleteCouponRPCHandler(ICouponRepositoryAsync couponRepository, IMapper mapper)
  {
    _couponRepository = couponRepository;
    _mapper = mapper;
  }

  public async Task<Response<int>> Handle(DeleteCouponRPC rpc)
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

    await _couponRepository.DeleteAsync(coupon);

    return new Response<int>(coupon.Id, "Coupon deleted");
  }
}
