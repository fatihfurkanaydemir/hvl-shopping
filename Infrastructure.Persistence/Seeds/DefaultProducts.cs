using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;

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
      var product1 = new Product
      {
        Name = "Product 1",
        Code = "#pr1",
        InStock = 50,
        Sold = 0,
        Status = ProductStatus.Active,
        Category = new Category { Name = "Category 1" },
      };

      var productList = await productRepository.GetAllAsync();
      var _product1 = productList.Where(c => c.Name.StartsWith(product1.Name)).Count();

      if (_product1 > 0) // ALREADY SEEDED
        return true;


      if (_product1 == 0)
        try
        {
          await productRepository.AddAsync(product1);
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
