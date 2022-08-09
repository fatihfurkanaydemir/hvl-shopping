using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
  public class ProfileInformation
  {
    public string Email { get; set; }
    public string Id { get; set; }
    public List<string> Roles { get; set; }
  }
}
