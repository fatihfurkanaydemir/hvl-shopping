using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using DiscountService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Infrastructure.Persistence.Repositories;

public class CouponRepositoryAsync : GenericRepositoryAsync<Coupon>, ICouponRepositoryAsync
{
  private readonly DbSet<Coupon> _coupons;

  public CouponRepositoryAsync(DiscountDbContext dbContext) : base(dbContext)
  {
    _coupons = dbContext.Coupons;
  }

  public async Task<IReadOnlyList<Coupon>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
  {
    return await _coupons
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .AsNoTracking()
          .ToListAsync();
  }
}
