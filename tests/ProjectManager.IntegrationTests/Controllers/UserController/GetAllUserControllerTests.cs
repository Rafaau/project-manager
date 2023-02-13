using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.UserController;

[Collection("Test collection")]
public class GetAllUserControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllUserControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task GetAll_ReturnsAllUsers_WhenSomeUsersExist()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    // Act
    var response = await _client.GetAsync("/api/user");

    // Assert
    var retrievedUsers = await response.Content.ReadFromJsonAsync<Response<UserSimplified[]>>();
    retrievedUsers!.Data.Should().ContainEquivalentOf(createdUser!.Data);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsEmptyResult_WhenNoUsersExist()
  {
    // Act
    var response = await _client.GetAsync("/api/user");

    // Assert
    var retrievedUsers = await response.Content.ReadFromJsonAsync<Response<UserSimplified[]>>();
    retrievedUsers!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public async Task DisposeAsync() => _resetDatabase();
}
