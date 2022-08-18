using DiscountService.Domain.Entities;

namespace DiscountService.Application.Interfaces.Repositories;

public interface IUsedCouponRepositoryAsync: IGenericRepositoryAsync<UsedCoupon>
{
  public Task<IReadOnlyList<UsedCoupon>> GetUsedCouponsByCustomerIdentityId(string id, int pageNumber, int pageSize);
  public Task<int> GetDataCountByCustomerIdentityId(string id);
  public Task<bool> DidCustomerUseCoupon(string customerIdentityId, int discountId);
  //public Task<IReadOnlyList<Order>> GetAllOrdersByCustomerIdentityIdAsync(string Id, int pageNumber, int pageSize);
  //public Task<IReadOnlyList<Order>> GetAllOrdersBySellerIdentityIdAsync(string Id, int pageNumber, int pageSize);
  //public Task<IReadOnlyList<Order>> GetOrdersByGroupId(string Id);
  //public Task<IReadOnlyList<Order>> GetOrdersByCheckoutSessionId(string Id);
  //public Task<int> GetDataCountByCustomerIdentityIdAsync(string Id);
  //public Task<int> GetDataCountBySellerIdentityIdAsync(string Id);
}
