using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface ICustomerRepositoryAsync: IGenericRepositoryAsync<Customer>
  {
    public Task<IReadOnlyList<Customer>> GetPagedReponseWithRelationsAsync(int pageNumber, int pageSize);
    public Task<Customer?> GetByIdentityIdWithRelationsAsync(string id);    
  }
}
