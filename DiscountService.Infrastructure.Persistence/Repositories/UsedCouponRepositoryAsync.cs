using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using DiscountService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Infrastructure.Persistence.Repositories;

public class UsedCouponRepositoryAsync : GenericRepositoryAsync<UsedCoupon>, IUsedCouponRepositoryAsync
{
  private readonly DbSet<UsedCoupon> _usedCoupons;

  public UsedCouponRepositoryAsync(DiscountDbContext dbContext) : base(dbContext)
  {
    _usedCoupons = dbContext.UsedCoupons;
  }

  public async Task<IReadOnlyList<UsedCoupon>> GetUsedCouponsByCustomerIdentityId(string id, int pageNumber, int pageSize)
  {
    return await _usedCoupons
          .Include(ud => ud.Coupon)
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .OrderByDescending(o => o.Created)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<int> GetDataCountByCustomerIdentityId(string id)
  {
    return await _usedCoupons
          .CountAsync(uc => uc.CustomerIdentityId == id);
  }

  public async Task<bool> DidCustomerUseCoupon(string customerIdentityId, int couponId)
  {
    return await _usedCoupons
          .Include(ud => ud.Coupon)
          .Where(ud => ud.CustomerIdentityId == customerIdentityId && ud.Coupon.Id == couponId)
          .AnyAsync();
  }
}
