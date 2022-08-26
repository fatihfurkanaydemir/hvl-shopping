using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface IProductRepositoryAsync: IGenericRepositoryAsync<Product>
  {
    public Task<IReadOnlyList<Product>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
    public Task<Product?> GetByIdWithRelationsAsync(int id);
    public Task<IReadOnlyList<Product>> GetByCategoryIdWithRelationsAsync(int id, int pageNumber, int pageSize);
    public Task<IReadOnlyList<Product>> GetBySellerIdentityIdWithRelationsAsync(string id, int pageNumber, int pageSize);
    public Task<IReadOnlyList<Product>> GetBySearchFilterWithRelationsAsync(string filterString, int pageNumber, int pageSize);
    public Task<int> GetDataCountByCategoryIdAsync(int id);
    public Task<int> GetDataCountBySellerIdentityIdAsync(string id);
    public Task<int> GetDataCountBySearchFilterAsync(string filterString);
    public Task DeleteImageAsync(Image image);
    public Task<Image> AddImageAsync(Image image);
    public Task<Image?> GetImageByIdAsync(int id);
    public Task<Image?> GetImageByUrlAsync(string url);
    public Task UpdateImageAsync(Image image);
  }
}
