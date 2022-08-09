using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
  public class AddressRepositoryAsync: GenericRepositoryAsync<Address>, IAddressRepositoryAsync
  {
    private readonly DbSet<Address> _addresses;

    public AddressRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
      _addresses = dbContext.Addresses;
    }
  }
}
