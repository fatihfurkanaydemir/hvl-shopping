using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface ISellerRepositoryAsync: IGenericRepositoryAsync<Seller>
  {
    public Task<Seller?> GetByIdentityIdAsync(string id);
    public Task<IEnumerable<Seller>?> GetPagedResponseWithRelationsAsync(int pageNumber, int pageSize);
    //public Task<Seller?> GetByIdentityIdWithRelationsAsync(string id);
  }
}
