using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
  public class Image: AuditableBaseEntity
  {
    public string Url { get; set; }
  }
}
