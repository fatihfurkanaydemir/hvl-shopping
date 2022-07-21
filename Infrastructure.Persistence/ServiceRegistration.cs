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
      Console.WriteLine(configuration.GetSection("UseInMemoryDatabase").Value);
      if (bool.Parse(configuration.GetSection("UseInMemoryDatabase").Value))
      {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("ApplicationDb"));
      }
      //else
      //{

      //  services.AddDbContext<ApplicationDbContext>(options =>
      // options.UseNpgsql(
      //     configuration.GetConnectionString("DefaultConnection"),
      //     b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
      //}
      services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
      services.AddScoped<IProductRepositoryAsync, ProductRepositoryAsync>();
      services.AddScoped<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
    }
  }
}
