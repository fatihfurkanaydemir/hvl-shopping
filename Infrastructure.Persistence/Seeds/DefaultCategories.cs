using Application.Interfaces.Repositories;
using Domain.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Seeds
{
  public class DefaultCategories
  {
    public static async Task<bool> SeedAsync(ICategoryRepositoryAsync categoryRepository)
    {
      var mockData = File.ReadAllText(Path.Combine(
        Directory.GetCurrentDirectory(),
        @"../Infrastructure.Persistence/Seeds/CATEGORY_MOCK_DATA.json"));

      var deserializedMockData = JsonConvert.DeserializeObject<List<Category>>(mockData);

      var _item1 = deserializedMockData[0];

      var itemList = await categoryRepository.GetAllAsync();
      var _itemCount = itemList.Where(i => i.Name.StartsWith(_item1.Name)).Count();

      if (_itemCount > 0) // ALREADY SEEDED
        return true;

      try
      {
        foreach (var deserializedItem in deserializedMockData)
        {
          await categoryRepository.AddAsync(deserializedItem);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }

      return true; // NOT ALREADY SEEDED
    }
  }
}
