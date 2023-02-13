using System;
using System.Collections.Generic;
using System.Data.Common;
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
using Npgsql;
using ProjectManager.Infrastructure;
using ProjectManager.Infrastructure.Data;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.Web;
using Respawn;
using Xunit;

namespace ProjectManager.IntegrationTests;
public class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
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

  private DbConnection _dbConnection = default!;
  private Respawner _respawner = default!;
  public HttpClient HttpClient { get; private set; } = default!;


  private readonly ApiServer _apiServer = new();

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
      services.RemoveAll<AppDbContext>();
      services.AddDbContext(_dbContainer.ConnectionString);
    });
  }

  public async Task InitializeAsync()
  {
    _apiServer.Start();
    _apiServer.SetupUser(Username);
    await _dbContainer.StartAsync();
    AppDbContextOptions.IsTesting = true;
    AppDbContextOptions.ConnectionString = _dbContainer.ConnectionString;
    _dbConnection = new NpgsqlConnection(_dbContainer.ConnectionString);
    HttpClient = CreateClient();
    await InitializeRespawner();
  }

  private async Task InitializeRespawner()
  {
    await _dbConnection.OpenAsync();
    _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
    {
      DbAdapter = DbAdapter.Postgres,
      SchemasToInclude = new[] { "public" }
    });
  }

  public async Task ResetDatabaseAsync()
  {
    await _respawner.ResetAsync(_dbConnection);
  }

  public new async Task DisposeAsync()
  {
    await _dbContainer.DisposeAsync();
    _apiServer.Dispose();
  }
}
