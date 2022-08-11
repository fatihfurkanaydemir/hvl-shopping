using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces.Repositories;

public interface IOrderRepositoryAsync: IGenericRepositoryAsync<Order>
{
  public Task<IReadOnlyList<Order>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
}
