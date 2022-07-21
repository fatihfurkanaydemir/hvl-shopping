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
          .AsNoTracking()
          .ToListAsync();
    }
  }
}
