﻿using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Entities;
using Common.Entities;
using OrderService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Infrastructure.Persistence.Repositories;

public class OrderRepositoryAsync : GenericRepositoryAsync<Order>, IOrderRepositoryAsync
{
  private readonly DbSet<Order> _orders;
  private readonly OrderDbContext _context;

  public OrderRepositoryAsync(OrderDbContext dbContext) : base(dbContext)
  {
    _orders = dbContext.Orders;
    _context = dbContext;
  }

  public async Task<IReadOnlyList<Order>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
  {
    return await _orders
          .Include(o => o.Products)
          .OrderByDescending(o => o.Created)
          .AsNoTracking()
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();
  }

  public async Task DeleteOrderProductAsync(OrderProduct product)
  {
    _context.Set<OrderProduct>().Remove(product);
    await _context.SaveChangesAsync();
  }

  public async Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdPagedAsync(string Id, int pageNumber, int pageSize)
  {
    return await _orders
          .OrderByDescending(o => o.Created)
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(o => o.Products)
          .Where(o => o.CustomerIdentityId == Id)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdAsync(string Id)
  {
    return await _orders
          .OrderByDescending(o => o.Created)
          .Include(o => o.Products)
          .Where(o => o.CustomerIdentityId == Id)
          .AsNoTracking()
          .ToListAsync();
  }


   public async Task<bool> DidCustomerBuyProduct(string identityId, int productId)
   {
     return await _orders
       .Include(o => o.Products)
       .AnyAsync(o => o.Products.Any(p => p.ProductId == productId));
   }

  public async Task<IReadOnlyList<Order>> GetAllOrdersBySellerIdentityIdAsync(string Id, int pageNumber, int pageSize)
  {
    return await _orders
          .OrderByDescending(o => o.Created)
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

  public async Task<Order?> GetByIdWithRelationsAsync(int Id)
  {
    return await _orders
          .Include(o => o.Products)
          .AsNoTracking()
          .SingleOrDefaultAsync(o => o.Id == Id);
  }

  public async Task<int> GetDataCountBySellerIdentityIdAsync(string Id)
  {
    return await _orders
          .Where(o => o.SellerIdentityId == Id)
          .AsNoTracking()
          .CountAsync();
  }

  public async Task<IReadOnlyList<Order>> GetOrdersByGroupId(string Id)
  {
    return await _orders
          .Include(o => o.Products)
          .OrderByDescending(o => o.Created)
          .Where(o => o.OrderGroupId == Id)
          .AsNoTracking()
          .ToListAsync();
  }

  public async Task<IReadOnlyList<Order>> GetOrdersByCheckoutSessionId(string Id)
  {
    return await _orders
          .Include(o => o.Products)
          .OrderByDescending(o => o.Created)
          .Where(o => o.CheckoutSessionId == Id)
          .AsNoTracking()
          .ToListAsync();
  }
}
