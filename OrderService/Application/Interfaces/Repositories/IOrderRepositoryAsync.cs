using Common.Entities;
using OrderService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces.Repositories;

public interface IOrderRepositoryAsync: IGenericRepositoryAsync<Order>
{
  public Task<IReadOnlyList<Order>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
  public Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdPagedAsync(string Id, int pageNumber, int pageSize);
  public Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdAsync(string Id);
  public Task<IReadOnlyList<Order>> GetAllOrdersBySellerIdentityIdAsync(string Id, int pageNumber, int pageSize);
  public Task<IReadOnlyList<Order>> GetOrdersByGroupId(string Id);
  public Task<IReadOnlyList<Order>> GetOrdersByCheckoutSessionId(string Id);
  public Task<int> GetDataCountByCustomerIdentityIdAsync(string Id);
  public Task<int> GetDataCountBySellerIdentityIdAsync(string Id);
  public Task<Order?> GetByIdWithRelationsAsync(int Id);
  public Task DeleteOrderProductAsync(OrderProduct product);
  public Task<bool> DidCustomerBuyProduct(string identityId, int productId);
}
