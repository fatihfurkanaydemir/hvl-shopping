using Common.Entities;
using Common.Enums;
using Common.EventBus.Events;

namespace Common.ApplicationEvents;

public class CreateOrderEvent : Event
{
  public string CustomerIdentityId { get; set; }
  public string CustomerFirstName { get; set; }
  public string CustomerLastName { get; set; }
  public string CustomerPhoneNumber { get; set; }
  public string SellerIdentityId { get; set; }
  public List<OrderProduct> Products { get; set; }
  public OrderStatus Status { get; set; }
  public string AddressTitle { get; set; }
  public string AddressDescription { get; set; }
  public string AddressCity { get; set; }
  public decimal TotalPrice { get; set; }
}
