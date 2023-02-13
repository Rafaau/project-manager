using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.UserController;

[Collection("Test collection")]
public class GetUserControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetUserControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Get_ReturnsUser_WhenUserExists()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    // Act
    var response = await _client.GetAsync($"/api/user/{createdUser!.Data.Email}");

    // Assert
    var retrievedUser = await response.Content.ReadFromJsonAsync<Response<UserComplex>>();
    retrievedUser!.Data.Should().BeEquivalentTo(createdUser.Data);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Get_ReturnsNotFound_WhenUserDoesNotExist()
  {
    // Act
    var response = await _client.GetAsync($"/api/user/notexisting");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public async Task DisposeAsync() => _resetDatabase();
}
