using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SharedViewModels
{
  public class ProductViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public string Status { get; set; }
    public CategoryViewModel Category { get; set; }
    public List<ImageViewModel> Images { get; set; }
    public SellerViewModel Seller { get; set; }
  }
}
