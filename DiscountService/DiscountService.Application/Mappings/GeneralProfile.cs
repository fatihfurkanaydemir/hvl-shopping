using AutoMapper;
using Common.ApplicationRPCs;
using Common.SharedViewModels;
using DiscountService.Domain.Entities;


namespace DiscountService.Application.Mappings
{
  public class GeneralProfile: Profile
  {
    public GeneralProfile()
    {
      CreateMap<Coupon, CouponViewModel>().ReverseMap();
      CreateMap<Coupon, CreateCouponRPC>().ReverseMap();
    }
  }
}
