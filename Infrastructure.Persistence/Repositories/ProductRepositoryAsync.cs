using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
  internal class ProductRepositoryAsync: GenericRepositoryAsync<Product>, IProductRepositoryAsync
  {
    private readonly DbSet<Product> _products;

    public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _products = dbContext.Products;
    }

    public async Task<IReadOnlyList<Product>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize)
    {
      return await _products
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(p => p.Images)
          .Include(p => p.Category)
          .AsNoTracking()
          .ToListAsync();
    }

    public async Task<Product?> GetByIdWithRelationsAsync(int id)
    {
      return await _products
        .Include(p => p.Images)
        .Include(p => p.Category)
        .AsTracking()
        .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdWithRelationsAsync(int id, int pageNumber, int pageSize)
    {
      return await _products
        .Include(p => p.Images)
        .Include(p => p.Category)
        .Where(p => p.Category.Id == id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<int> GetDataCountByCategoryIdAsync(int id)
    {
      return await _products
        .Include(p => p.Category)
        .Include(p => p.Images)
        .Where(p => p.Category.Id == id)
        .CountAsync();
    }
  }
}
