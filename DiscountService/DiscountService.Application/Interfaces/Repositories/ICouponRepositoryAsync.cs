using DiscountService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscountService.Application.Interfaces.Repositories;

public interface ICouponRepositoryAsync: IGenericRepositoryAsync<Coupon>
{
  public Task<IReadOnlyList<Coupon>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
}
