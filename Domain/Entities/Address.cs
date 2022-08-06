using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Address: AuditableBaseEntity
  {
    public string AddressDescription { get; set; }
    public string City { get; set; }

  }
}
