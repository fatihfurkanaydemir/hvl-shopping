using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
  internal class SellerRepositoryAsync: GenericRepositoryAsync<Seller>, ISellerRepositoryAsync
  {
    private readonly DbSet<Seller> _sellers;

    public SellerRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _sellers = dbContext.Sellers;
    }

    public async Task<Seller?> GetByIdentityIdAsync(string id)
    {
      return await _sellers
        .AsNoTracking()
        .SingleOrDefaultAsync(s => s.IdentityId == id);
    }

    public async Task<IEnumerable<Seller>?> GetPageResponseWithRelationsAsync(int pageNumber, int pageSize)
    {
      return await _sellers
        .Include(s => s.Address)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
    }
  }
}
