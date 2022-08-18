using AutoMapper;
using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using Common.SharedViewModels;
using Common.Wrappers;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class GetAllCouponsRPCHandler : IRPCHandler<GetAllCouponsRPC, PagedResponse<IEnumerable<CouponViewModel>>>
{
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IMapper _mapper;
  public GetAllCouponsRPCHandler(ICouponRepositoryAsync couponRepository, IMapper mapper)
  {
    _couponRepository = couponRepository;
    _mapper = mapper;
  }

  public async Task<PagedResponse<IEnumerable<CouponViewModel>>> Handle(GetAllCouponsRPC rpc)
  {
    var coupons = await _couponRepository.GetPagedReponseAsync(rpc.PageNumber, rpc.PageSize);
    var dataCount = await _couponRepository.GetDataCount();

    var viewModels = new List<CouponViewModel>();

    foreach(var c in coupons)
    {
      viewModels.Add(_mapper.Map<CouponViewModel>(c));
    }

    return new PagedResponse<IEnumerable<CouponViewModel>> (viewModels, rpc.PageNumber, rpc.PageSize, dataCount);
  }
}
