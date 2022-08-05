using Application.Features.SharedViewModels;

namespace Application.Features.Categories.Queries.GetCategoryProductsById
{
  public class GetCategoryProductsByIdProductViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int InStock { get; set; }
    public int Sold { get; set; }
    public string Status { get; set; }
    public List<GetCategoryProductsByIdImageViewModel> Images { get; set; }
    public SellerViewModel Seller { get; set; }
  }
}
