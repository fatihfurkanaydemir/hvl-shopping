using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Address: AuditableBaseEntity
  {
    public string Title { get; set; }
    public string AddressDescription { get; set; }
    public string City { get; set; }
  }
}
