using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.UserController.UserGenerator;

namespace ProjectManager.IntegrationTests.UserController;
public class GetUserControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
{
  private readonly HttpClient _client;
  private List<int> _idsToDelete = new();

  public GetUserControllerTests(ApiFactory factory)
  {
    AppDbContextOptions.IsTesting = true;
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Get_ReturnsUser_WhenUserExists()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<UserSimplified>();
    _idsToDelete.Add(createdUser!.Id);

    // Act
    var response = await _client.GetAsync($"/api/user/{createdUser!.Email}");

    // Assert
    var retrievedUser = await response.Content.ReadFromJsonAsync<Response<UserComplex>>();
    retrievedUser!.Data.Should().BeEquivalentTo(createdUser);
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

  public async Task DisposeAsync()
  {
    foreach (var id in _idsToDelete)
    {
      await _client.DeleteAsync($"/api/user/{id}");
    }
  }
}
