using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
  internal class CustomerRepositoryAsync: GenericRepositoryAsync<Customer>, ICustomerRepositoryAsync
  {
    private readonly DbSet<Customer> _customers;

    public CustomerRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _customers = dbContext.Customers;
    }

    public async Task<IReadOnlyList<Customer>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
    {
      return await _customers
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(p => p.Addresses)
          .AsNoTracking()
          .ToListAsync();
    }

    public async Task<Customer?> GetByIdentityIdWithRelationsAsync(string id)
    {
      return await _customers
        .Include(p => p.Addresses)
        .AsTracking()
        .SingleOrDefaultAsync(p => p.IdentityId == id);
    }
  }
}
