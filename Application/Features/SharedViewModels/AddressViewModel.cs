using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SharedViewModels
{
  public class AddressViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string AddressDescription { get; set; }
    public string City { get; set; }
  }
}
