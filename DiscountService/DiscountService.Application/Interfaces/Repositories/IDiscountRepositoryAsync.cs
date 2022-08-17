using DiscountService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscountService.Application.Interfaces.Repositories;

public interface IDiscountRepositoryAsync: IGenericRepositoryAsync<Discount>
{
  public Task<IReadOnlyList<Discount>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
}
