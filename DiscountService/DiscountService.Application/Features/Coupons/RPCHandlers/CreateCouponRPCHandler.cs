using AutoMapper;
using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using Common.Wrappers;
using Common.ApplicationEvents;

namespace DiscountService.Application.Features.Coupons.RPCHandlers;

public class CreateCouponRPCHandler : IRPCHandler<CreateCouponRPC, Response<int>>
{
  private readonly IEventBus _eventBus;
  private readonly ICouponRepositoryAsync _couponRepository;
  private readonly IMapper _mapper;
  public CreateCouponRPCHandler(ICouponRepositoryAsync couponRepository, IMapper mapper, IEventBus eventBus)
  {
    _couponRepository = couponRepository;
    _mapper = mapper;
    _eventBus = eventBus;
  }

  public async Task<Response<int>> Handle(CreateCouponRPC rpc)
  {
    var existingCoupon = await _couponRepository.GetByCodeAsync(rpc.Code);
    if(existingCoupon != null)
    {
      return new Response<int>
      {
        Succeeded = false,
        Message = "Coupon code already exists"
      };
    }

    var coupon = await _couponRepository.AddAsync(_mapper.Map<Coupon>(rpc));

    _eventBus.Publish(new DiscountCouponCreatedEvent 
    {
      CouponCode = coupon.Code,
      Amount = coupon.Amount
    });

    return new Response<int>(coupon.Id, "Coupon created");
  }
}
