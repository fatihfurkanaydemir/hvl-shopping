using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
  public class CategoryRepositoryAsync: GenericRepositoryAsync<Category>, ICategoryRepositoryAsync
  {
    private readonly DbSet<Category> _categories;

    public CategoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _categories = dbContext.Categories;
    }

    public async Task<Category?> GetByIdWithRelationsAsync(int id)
    {
      return await _categories
        .Include(c => c.Products)
        .AsNoTracking()
        .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
      return await _categories
        .AsNoTracking()
        .Include(c => c.Products)
        .SingleOrDefaultAsync(c => c.Name == name);
    }
  }
}
