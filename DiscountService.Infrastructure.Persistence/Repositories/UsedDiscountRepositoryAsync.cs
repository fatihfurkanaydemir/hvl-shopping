using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using DiscountService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Infrastructure.Persistence.Repositories;

public class UsedDiscountRepositoryAsync : GenericRepositoryAsync<Discount>, IUsedDiscountRepositoryAsync
{
  private readonly DbSet<UsedDiscount> _usedDiscounts;

  public UsedDiscountRepositoryAsync(DiscountDbContext dbContext) : base(dbContext)
  {
    _usedDiscounts = dbContext.UsedDiscounts;
  }

  public async Task<IReadOnlyList<UsedDiscount>> GetUsedDiscountsByCustomerIdentityId(string id, int pageNumber, int pageSize)
  {
    return await _usedDiscounts
          .Include(ud => ud.Discount)
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .OrderByDescending(o => o.Created)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<bool> DidCustomerUseDiscount(string customerIdentityId, int discountId)
  {
    return await _usedDiscounts
          .Include(ud => ud.Discount)
          .Where(ud => ud.CustomerIdentityId == customerIdentityId && ud.Discount.Id == discountId)
          .AnyAsync();
  }
}
