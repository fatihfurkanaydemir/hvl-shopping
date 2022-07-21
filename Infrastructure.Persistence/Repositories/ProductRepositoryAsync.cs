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
  }
}
