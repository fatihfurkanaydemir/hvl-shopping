using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface IProductRepositoryAsync: IGenericRepositoryAsync<Product>
  {
    public Task<IReadOnlyList<Product>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
    public Task<Product?> GetByIdWithRelationsAsync(int id);

    public Task<IReadOnlyList<Product>> GetByCategoryIdWithRelationsAsync(int id, int pageNumber, int pageSize);
    public Task<IReadOnlyList<Product>> GetBySellerIdentityIdWithRelationsAsync(string id, int pageNumber, int pageSize);
    public Task<int> GetDataCountByCategoryIdAsync(int id);
    public Task<int> GetDataCountBySellerIdentityIdAsync(string id);
  }
}
