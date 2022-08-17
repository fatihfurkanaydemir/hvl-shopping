using DiscountService.Domain.Entities;

namespace DiscountService.Application.Interfaces.Repositories;

public interface IUsedDiscountRepositoryAsync: IGenericRepositoryAsync<Discount>
{
  public Task<IReadOnlyList<UsedDiscount>> GetUsedDiscountsByCustomerIdentityId(string id, int pageNumber, int pageSize);
  public Task<bool> DidCustomerUseDiscount(string customerIdentityId, int discountId);
  //public Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdAsync(string Id, int pageNumber, int pageSize);
  //public Task<IReadOnlyList<Order>> GetAllOrdersBySellerIdentityIdAsync(string Id, int pageNumber, int pageSize);
  //public Task<IReadOnlyList<Order>> GetOrdersByGroupId(string Id);
  //public Task<IReadOnlyList<Order>> GetOrdersByCheckoutSessionId(string Id);
  //public Task<int> GetDataCountByCustomerIdentityIdAsync(string Id);
  //public Task<int> GetDataCountBySellerIdentityIdAsync(string Id);
}
