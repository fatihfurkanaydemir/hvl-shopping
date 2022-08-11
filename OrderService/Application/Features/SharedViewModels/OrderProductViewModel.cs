using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Features.SharedViewModels;

public class OrderProductViewModel
{
  public int ProductId { get; set; }
  public string ProductName { get; set; }
  public int Count { get; set; }
  public decimal PricePerProduct { get; set; }
}
