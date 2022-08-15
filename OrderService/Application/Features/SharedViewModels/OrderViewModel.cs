using OrderService.Application.Features.SharedViewModels;

namespace OrderService.Application.Features.SharedViewModels;

public class OrderViewModel
{
  public string CustomerIdentityId { get; set; }
  public string CustomerFirstName { get; set; }
  public string CustomerLastName { get; set; }
  public string CustomerPhoneNumber { get; set; }
  public string SellerIdentityId { get; set; }
  public List<OrderProductViewModel> Products { get; set; }
  public DateTime Created { get; set; }
  public string Status { get; set; }
  public string AddressTitle { get; set; }
  public string AddressDescription { get; set; }
  public string AddressCity { get; set; }
  public Decimal TotalPrice { get; set; }
}
