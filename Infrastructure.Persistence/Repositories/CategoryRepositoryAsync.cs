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
  }
}
