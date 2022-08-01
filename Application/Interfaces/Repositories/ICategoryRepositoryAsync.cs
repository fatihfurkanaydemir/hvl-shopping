using Domain.Entities;

namespace Application.Interfaces.Repositories
{
  public interface ICategoryRepositoryAsync: IGenericRepositoryAsync<Category>
  {
    Task<Category?> GetByIdWithRelationsAsync(int id);
    Task<Category?> GetByNameAsync(string name);
  }
}
