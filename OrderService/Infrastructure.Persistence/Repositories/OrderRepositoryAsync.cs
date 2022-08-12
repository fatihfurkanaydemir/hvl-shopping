using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Infrastructure.Persistence.Repositories;

public class OrderRepositoryAsync : GenericRepositoryAsync<Order>, IOrderRepositoryAsync
{
  private readonly DbSet<Order> _orders;

  public OrderRepositoryAsync(OrderDbContext dbContext) : base(dbContext)
  {
    _orders = dbContext.Orders;
  }

  public async Task<IReadOnlyList<Order>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
  {
    return await _orders
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(o => o.Products)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdAsync(string Id, int pageNumber, int pageSize)
  {
    return await _orders
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(o => o.Products)
          .Where(o => o.CustomerIdentityId == Id)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<IReadOnlyList<Order>> GetAllOrdersBySellerIdentityIdAsync(string Id, int pageNumber, int pageSize)
  {
    return await _orders
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(o => o.Products)
          .Where(o => o.SellerIdentityId == Id)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<int> GetDataCountByCustomerIdentityIdAsync(string Id)
  {
    return await _orders
          .Where(o => o.CustomerIdentityId == Id)
          .AsNoTracking()
          .CountAsync();
  }

  public async Task<int> GetDataCountBySellerIdentityIdAsync(string Id)
  {
    return await _orders
          .Where(o => o.SellerIdentityId == Id)
          .AsNoTracking()
          .CountAsync();
  }
}
