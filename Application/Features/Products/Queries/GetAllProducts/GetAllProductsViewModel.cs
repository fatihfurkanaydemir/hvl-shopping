using Domain.Entities;
using Domain.Enums;
using Application.Features.SharedViewModels;

namespace Application.Features.Products.Queries.GetAllProducts
{
  public class GetAllProductsViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public string Status { get; set; }
    public List<GetAllProductsImageViewModel> Images { get; set; }
    public GetAllProductsCategoryViewModel Category { get; set; }
    public SellerViewModel Seller { get; set; }
  }
}
