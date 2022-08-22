using NotificationService.Application.Interfaces.Repositories;
using NotificationService.Infrastructure.Persistence.Contexts;
using NotificationService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Infrastructure.Persistence;

public static class ServiceRegistration
{
  public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    if (bool.Parse(configuration.GetSection("UseInMemoryDatabase").Value))
    {
      services.AddDbContext<NotificationDbContext>(options =>
          options.UseInMemoryDatabase("DiscountDb"));
    }
    else
    {
      services.AddDbContext<NotificationDbContext>(options =>
      options.UseNpgsql(
         configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)));
    }

    services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
    //services.AddScoped<INotificationRepositoryAsync, NotificationRepositoryAsync>();
  }
}
