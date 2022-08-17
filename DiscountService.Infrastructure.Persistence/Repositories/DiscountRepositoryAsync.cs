using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Domain.Entities;
using DiscountService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DiscountService.Infrastructure.Persistence.Repositories;

public class DiscountRepositoryAsync : GenericRepositoryAsync<Discount>, IDiscountRepositoryAsync
{
  private readonly DbSet<Discount> _discounts;

  public DiscountRepositoryAsync(DiscountDbContext dbContext) : base(dbContext)
  {
    _discounts = dbContext.Discounts;
  }

  public async Task<IReadOnlyList<Discount>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
  {
    return await _discounts
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .AsNoTracking()
          .ToListAsync();
  }
}
