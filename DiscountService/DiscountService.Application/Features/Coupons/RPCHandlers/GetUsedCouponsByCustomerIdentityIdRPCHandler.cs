using AutoMapper;
using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using Common.SharedViewModels;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class GetUsedCouponsByCustomerIdentityIdRPCHandler : IRPCHandler<GetUsedCouponsByCustomerIdentityIdRPC, PagedResponse<IEnumerable<CouponViewModel>>>
{
  private readonly IUsedCouponRepositoryAsync _usedCouponRepository;
  private readonly IMapper _mapper;
  public GetUsedCouponsByCustomerIdentityIdRPCHandler(IUsedCouponRepositoryAsync usedCouponRepository, IMapper mapper)
  {
    _usedCouponRepository = usedCouponRepository;
    _mapper = mapper;
  }

  public async Task<PagedResponse<IEnumerable<CouponViewModel>>> Handle(GetUsedCouponsByCustomerIdentityIdRPC rpc)
  {
    var usedCoupons = await _usedCouponRepository.GetUsedCouponsByCustomerIdentityId(rpc.CustomerIdentityId, rpc.PageNumber, rpc.PageSize);
    var dataCount = await _usedCouponRepository.GetDataCountByCustomerIdentityId(rpc.CustomerIdentityId);

    var viewModels = new List<CouponViewModel>();

    foreach(var uc in usedCoupons)
    {
      viewModels.Add(_mapper.Map<CouponViewModel>(uc.Coupon));
    }

    return new PagedResponse<IEnumerable<CouponViewModel>> (viewModels, rpc.PageNumber, rpc.PageSize, dataCount);
  }
}
