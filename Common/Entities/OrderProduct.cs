
namespace Common.Entities;

public class OrderProduct: AuditableBaseEntity
{
  public int ProductId { get; set; }
  public string ProductName { get; set; }
  public int Count { get; set; }
  public decimal PricePerProduct { get; set; }
}
