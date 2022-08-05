using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface ISellerRepositoryAsync: IGenericRepositoryAsync<Seller>
  {
    public Task<Seller?> GetByIdentityIdAsync(string id);
    public Task<IEnumerable<Seller>?> GetPageResponseWithRelationsAsync(int pageNumber, int pageSize);
  }
}
