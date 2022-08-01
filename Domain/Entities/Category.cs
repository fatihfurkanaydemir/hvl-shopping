using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Common;

namespace Domain.Entities
{
  public class Category: AuditableBaseEntity
  {
    public string Name { get; set; }
    public List<Product> Products { get; set; }
  }
}
