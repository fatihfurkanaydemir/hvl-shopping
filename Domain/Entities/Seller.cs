using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Seller: AuditableBaseEntity
  {
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string ShopName { get; set; }
    public Address Address { get; set; }
    public List<Product> Products { get; set; }
  }
}