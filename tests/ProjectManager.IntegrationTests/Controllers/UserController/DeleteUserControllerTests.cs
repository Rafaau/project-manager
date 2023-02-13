using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.UserController;

[Collection("Test collection")]
public class DeleteUserControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public DeleteUserControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenUserExists()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    // Act
    var response = await _client.DeleteAsync($"/api/user/{createdUser!.Data.Id}");

    // Assert
    var deletedUser = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    deletedUser!.Data.Should().BeEquivalentTo(createdUser.Data);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync("/api/user/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
