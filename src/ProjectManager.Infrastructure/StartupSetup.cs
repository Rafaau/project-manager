using ProjectManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using ProjectManager.Infrastructure.Data.Config;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ProjectManager.Infrastructure;

public static class StartupSetup
{
  public static void AddDbContext(this IServiceCollection services) =>
      services.AddDbContext<AppDbContext>(options =>
      {
        if (AppDbContextOptions.IsTesting)
          options.UseNpgsql("Server=localhost;Port=5432;Database=projectmanagerDbTests;User Id=postgres;Password=postgrespw;Pooling=false");
        //options.UseInMemoryDatabase("projectmanagerDb");
        else
          options.UseNpgsql("Server=localhost;Port=5432;Database=projectmanagerDb;User Id=postgres;Password=postgrespw;Pooling=false",
            b => b.MigrationsAssembly("ProjectManager.Infrastructure"));
      });

  public static void AddDbContext(this IServiceCollection services, string connectionString) =>
    services.AddDbContext<AppDbContext>(options =>
    {
      options.UseNpgsql(connectionString);
    });
}
