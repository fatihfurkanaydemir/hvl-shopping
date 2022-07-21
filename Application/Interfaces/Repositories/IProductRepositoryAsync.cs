using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface IProductRepositoryAsync: IGenericRepositoryAsync<Product>
  {
    public Task<IReadOnlyList<Product>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
    public Task<Product> GetByIdWithRelationsAsync(int id);
  }
}
