using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Seeds
{
  public class DefaultCategories
  {
    public static async Task<bool> SeedAsync(ICategoryRepositoryAsync categoryRepository)
    {
      var category1 = new Category
      {
        Name = "Category 1",
      };

      var categoryList = await categoryRepository.GetAllAsync();
      var _category1 = categoryList.Where(c => c.Name.StartsWith(category1.Name)).Count();

      if (_category1 > 0) // ALREADY SEEDED
        return true;


      if (_category1 == 0)
        try
        {
          await categoryRepository.AddAsync(category1);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          throw;
        }

      return false; // NOT ALREADY SEEDED
    }
  }
}
