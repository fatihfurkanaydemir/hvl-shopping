using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeds
{
  public class DefaultProducts
  {
    public static async Task<bool> SeedAsync(IProductRepositoryAsync productRepository)
    {
      var mockData = File.ReadAllText(Path.Combine(
        Directory.GetCurrentDirectory(),
        @"../Infrastructure.Persistence/Seeds/PRODUCT_MOCK_DATA.json"));

      var deserializedMockData = JsonConvert.DeserializeObject<List<Product>>(mockData);

      var _item1 = deserializedMockData[0];

      var itemList = await productRepository.GetAllAsync();
      var _itemCount = itemList.Where(i => i.Name.StartsWith(_item1.Name)).Count();

      if (_itemCount > 0) // ALREADY SEEDED
        return true;

      try
      {
        foreach (var deserializedItem in deserializedMockData)
        {
          await productRepository.AddAsync(deserializedItem);
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
