using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.UserController.UserGenerator;

namespace ProjectManager.IntegrationTests.UserController;
public class GetAllUserControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
{
  private readonly HttpClient _client;
  private List<int> _idsToDelete = new();

  public GetAllUserControllerTests(ApiFactory factory)
  {
    AppDbContextOptions.IsTesting = true;
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GetAll_ReturnsAllUsers_WhenUsersExist()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<UserSimplified>();
    _idsToDelete.Add(createdUser!.Id);

    // Act
    var response = await _client.GetAsync("/api/user");

    // Assert
    var retrievedUsers = await response.Content.ReadFromJsonAsync<Response<UserSimplified[]>>();
    retrievedUsers!.Data.Should().ContainEquivalentOf(createdUser);
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

  public async Task DisposeAsync()
  {
    foreach (var id in _idsToDelete)
    {
      await _client.DeleteAsync($"/api/user/{id}");
    }
  }
}
