using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ProjectManager.Infrastructure.Data;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.Web;

namespace ProjectManager.IntegrationTests;
public class ApiFactory : WebApplicationFactory<IApiMarker>
{
  public const string Username = "Rafau";

  private readonly TestcontainerDatabase _dbContainer =
    new TestcontainersBuilder<PostgreSqlTestcontainer>()
      .WithDatabase(new PostgreSqlTestcontainerConfiguration
      {
        Database = "projectmanagerDbTests",
        Username = "postgres",
        Password = "postgrespw",
      }).Build();

  private readonly ApiServer _apiServer = new ();

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureLogging(logging =>
    {
      logging.ClearProviders();
    });

    builder.ConfigureTestServices(services =>
    {
      services.AddHttpClient("api", cfg =>
      {
        cfg.BaseAddress = new Uri(_apiServer.Url);
      });

      services.RemoveAll(typeof(AppDbContext));
      services.AddDbContext<AppDbContext>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=projectmanagerDbTests;User Id=postgres;Password=postgrespw;Pooling=false"));
    });
  }

  public async Task InitializeAsync()
  {
    _apiServer.Start();
    _apiServer.SetupUser(Username);
    await _dbContainer.StartAsync();
  }

  public new async Task DisposeAsync()
  {
    await _dbContainer.DisposeAsync();
    _apiServer.Dispose();
  }
}
