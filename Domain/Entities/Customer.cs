using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Customer: AuditableBaseEntity
  {
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public List<Address> Addresses { get; set; }
  }
}
