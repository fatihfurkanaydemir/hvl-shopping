using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Persistence
{
  public static class ServiceRegistration
  {
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      if (bool.Parse(configuration.GetSection("UseInMemoryDatabase").Value))
      {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ApplicationDb"));
      }
      else
      {

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(
           configuration.GetConnectionString("DefaultConnection"),
           b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
      }

      services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
      services.AddScoped<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
      services.AddScoped<IBasketRepositoryAsync, BasketRepositoryAsync>();
      services.AddScoped<IProductRepositoryAsync, ProductRepositoryAsync>();
      services.AddScoped<ICustomerRepositoryAsync, CustomerRepositoryAsync>();
      services.AddScoped<ISellerRepositoryAsync, SellerRepositoryAsync>();
      services.AddScoped<IAddressRepositoryAsync, AddressRepositoryAsync>();
    }
  }
}
