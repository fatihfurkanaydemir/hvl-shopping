using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal class OrderRepositoryAsync: GenericRepositoryAsync<Order>, IOrderRepositoryAsync
{
  private readonly DbSet<Order> _orders;

  public OrderRepositoryAsync(OrderDbContext dbContext) : base(dbContext)
  {
    _orders = dbContext.Orders;
  }

  //public async Task<IReadOnlyList<Customer>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
  //{
  //  return await _customers
  //      .Skip((pageNumber - 1) * pageSize)
  //      .Take(pageSize)
  //      .Include(p => p.Addresses)
  //      .AsNoTracking()
  //      .ToListAsync();
  //}

  //public async Task<Customer?> GetByIdentityIdWithRelationsAsync(string id)
  //{
  //  return await _customers
  //    .Include(p => p.Addresses)
  //    .AsNoTracking()
  //    .SingleOrDefaultAsync(p => p.IdentityId == id);
  //}
}
