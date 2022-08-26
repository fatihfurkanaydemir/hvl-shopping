using Application.Parameters;

namespace Application.Features.Products.Queries.GetProductsBySearchFilter
{
  public class GetProductsBySearchFilterParameter : RequestParameter
  {
    public string FilterString { get; set; }
  }
}
