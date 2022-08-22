using OrderService.Application.Features.SharedViewModels;

namespace OrderService.Application.Features.SharedViewModels;

public class OrderViewModel
{
  public int Id { get; set; }
  public string CustomerIdentityId { get; set; }
  public string CustomerFirstName { get; set; }
  public string CustomerLastName { get; set; }
  public string CustomerPhoneNumber { get; set; }
  public string SellerIdentityId { get; set; }
  public string OrderGroupId { get; set; }
  public string CheckoutSessionId { get; set; }
  public string PaymentIntentId { get; set; }
  public List<OrderProductViewModel> Products { get; set; }
  public DateTime Created { get; set; }
  public string Status { get; set; }
  public string AddressTitle { get; set; }
  public string AddressDescription { get; set; }
  public string AddressCity { get; set; }
  public decimal TotalProductPrice { get; set; }
  public decimal ShipmentPrice { get; set; }
  public string CouponCode { get; set; }
  public decimal CouponAmount { get; set; }
}
