using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using WireMock.ResponseBuilders;
using Xunit;
using static ProjectManager.IntegrationTests.ProjectController.ProjectGenerator;
using static ProjectManager.IntegrationTests.UserController.UserGenerator;

namespace ProjectManager.IntegrationTests.ProjectController;
public class GetAllProjectControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
{
  private readonly HttpClient _client;
  private List<int> _projectIdsToDelete = new();
  private List<int> _userIdsToDelete = new();
  public GetAllProjectControllerTests(ApiFactory factory)
  {
    AppDbContextOptions.IsTesting = true;
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GetAll_ReturnsAllProjects_WhenProjectsExist()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    var user = _userGenerator.Generate();
    var createdUserResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdUserResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    project.ManagerId = createdUser!.Data.Id;
    var createdProjectResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdProjectResponse.Content.ReadFromJsonAsync<Response<ProjectSimplified>>();
    _projectIdsToDelete.Add(createdProject!.Data.Id);
    _userIdsToDelete.Add(createdUser!.Data.Id);

    // Act
    var response = await _client.GetAsync("/api/project");

    // Assert
    var retrievedProjects = await response.Content.ReadFromJsonAsync<Response<ProjectSimplified[]>>();
    retrievedProjects!.Data.Should().ContainEquivalentOf(createdProject.Data);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsEmptyResult_WhenNoProjectsExist()
  {
    // Act
    var response = await _client.GetAsync("/api/project");

    // Assert
    var retrievedProjects = await response.Content.ReadFromJsonAsync<Response<ProjectSimplified[]>>();
    retrievedProjects!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
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
