using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Seeds
{
  public class DefaultSellers
  {
    public static async Task<bool> SeedAsync(ISellerRepositoryAsync sellerRepository, AuthService authService)
    {
      var mockData = File.ReadAllText(Path.Combine(
        Directory.GetCurrentDirectory(),
        @"../Infrastructure.Persistence/Seeds/SELLER_MOCK_DATA.json"));

      var deserializedMockData = JsonConvert.DeserializeObject<List<Seller>>(mockData);

      var _item1 = deserializedMockData[0];

      var itemList = await sellerRepository.GetAllAsync();
      var _itemCount = itemList.Where(i => i.FirstName.StartsWith(_item1.FirstName)).Count();

      if (_itemCount > 0) // ALREADY SEEDED
        return true;

      try
      {
        int i = 0;
        foreach (var deserializedItem in deserializedMockData)
        {
          var email = $"s{i}@h.com";
          var password = "123Asd.";

          var registerResponse = await authService.RegisterSeller(email, password, password);

          deserializedItem.IdentityId = registerResponse.Data;

          await sellerRepository.AddAsync(deserializedItem);
          ++i;
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
