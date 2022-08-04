using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Seller: AuditableBaseEntity
  {
    public string Name { get; set; }
    
  }
}
