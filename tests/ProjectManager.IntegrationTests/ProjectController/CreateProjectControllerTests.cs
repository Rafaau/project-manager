using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Infrastructure.Data.Config;
using Xunit;

namespace ProjectManager.IntegrationTests.ProjectController;
public class CreateProjectControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
{
  private readonly HttpClient _client;
  private List<int> _projectIdsToDelete = new();
  private List<int> _userIdsToDelete = new();

  public CreateProjectControllerTests(ApiFactory factory)
  {
    AppDbContextOptions.IsTesting = true;
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Create_CreatesProject_WhenDataIsValid()
  {
    // Arrange


    // Act


    // Assert

  }

  public Task InitializeAsync() => Task.CompletedTask;

  public async Task DisposeAsync()
  {
    foreach (var id in _projectIdsToDelete)
    {
      await _client.DeleteAsync($"/api/project/{id}");
    }

    foreach (var id in _userIdsToDelete)
    {
      await _client.DeleteAsync($"/api/user/{id}");
    }
  }
}
