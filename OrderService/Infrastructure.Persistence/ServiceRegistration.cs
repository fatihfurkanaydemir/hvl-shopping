using Application.Interfaces.Repositories;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class ServiceRegistration
{
  public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    if (bool.Parse(configuration.GetSection("UseInMemoryDatabase").Value))
    {
      services.AddDbContext<OrderDbContext>(options =>
          options.UseInMemoryDatabase("OrderDb"));
    }
    else
    {

      services.AddDbContext<OrderDbContext>(options =>
      options.UseNpgsql(
         configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)));
    }

    services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
    services.AddScoped<IOrderRepositoryAsync, OrderRepositoryAsync>();
  }
}
