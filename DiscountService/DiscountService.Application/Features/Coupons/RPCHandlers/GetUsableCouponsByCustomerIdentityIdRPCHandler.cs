using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using Common.Wrappers;
using Common.SharedViewModels;
using AutoMapper;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class GetUsableCouponsByCustomerIdentityIdRPCHandler : IRPCHandler<GetUsableCouponsByCustomerIdentityIdRPC, Response<IEnumerable<CouponViewModel>>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IUsedCouponRepositoryAsync _usedCouponRepository;
  private readonly IMapper _mapper;
  public GetUsableCouponsByCustomerIdentityIdRPCHandler(ICouponRepositoryAsync couponRepository, IUsedCouponRepositoryAsync usedCouponRepository, IMapper mapper)
  {
    _couponRepository = couponRepository;
    _usedCouponRepository = usedCouponRepository;
    _mapper = mapper;
  }

  public async Task<Response<IEnumerable<CouponViewModel>>> Handle(GetUsableCouponsByCustomerIdentityIdRPC rpc)
  {
    var usedCoupons = await _usedCouponRepository.GetUsedCouponsByCustomerIdentityId(rpc.CustomerIdentityId);
    var usableCoupons = await _couponRepository.GetUsableCouponsAsync(usedCoupons);

    var viewModels = new List<CouponViewModel>();

    foreach(var usableCoupon in usableCoupons)
    {
      viewModels.Add(_mapper.Map<CouponViewModel>(usableCoupon));
    }

    return new Response<IEnumerable<CouponViewModel>>(viewModels, "Usable coupons");
  }
}
