using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Builders;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System.Data.Common;
using Respawn;

namespace ProjectManager.E2ETests;
public class SharedTestContext : IAsyncLifetime
{
  public IWebDriver Driver;
  public readonly WebDriverWait Wait;
  public const string AppUrl = "http://localhost:7780/";
  private readonly ApiServer _apiServer = new();

  private static readonly string DockerComposeFile =
    Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"../../../docker-compose.integration.yml");

  private readonly ICompositeService _dockerService = new Builder()
    .UseContainer()
    .UseCompose()
    .FromFile(DockerComposeFile)
    .RemoveOrphans()
    .WaitForHttp("test-app", AppUrl)
    .Build();

  public SharedTestContext()
  {
    Driver = new ChromeDriver();
    Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
    Driver.Manage().Window.Maximize();
  }

  private DbConnection _dbConnection = default!;
  private Respawner _respawner = default!;

  public async Task ResetDriver()
  {
    ((IJavaScriptExecutor)Driver).ExecuteScript("window.localStorage.clear();");
    //Driver.SwitchTo().NewWindow(WindowType.Window);
    //await _respawner.ResetAsync(_dbConnection);
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

  public async Task InitializeAsync()
  {
    _apiServer.Start();
    _dockerService.Start();
    await Task.Delay(2000);
    //_dbConnection = new NpgsqlConnection("Server=localhost;Port=5432;Database=projectmanagerDb;User ID=postgres;Password=postgrespw;");
    //await InitializeRespawner();
  }

  public async Task DisposeAsync()
  {
    _dockerService.Dispose();
    //Driver.Close();
    Driver.Quit();
    _apiServer.Dispose();
  }
}
