using DiscountService.Application.Interfaces.Repositories;
using DiscountService.Infrastructure.Persistence.Contexts;
using DiscountService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountService.Infrastructure.Persistence;

public static class ServiceRegistration
{
  public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    if (bool.Parse(configuration.GetSection("UseInMemoryDatabase").Value))
    {
      services.AddDbContext<DiscountDbContext>(options =>
          options.UseInMemoryDatabase("DiscountDb"));
    }
    else
    {
      services.AddDbContext<DiscountDbContext>(options =>
      options.UseNpgsql(
         configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly(typeof(DiscountDbContext).Assembly.FullName)));
    }

    services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
    services.AddScoped<ICouponRepositoryAsync, CouponRepositoryAsync>();
    services.AddScoped<IUsedCouponRepositoryAsync, UsedCouponRepositoryAsync>();
  }
}
