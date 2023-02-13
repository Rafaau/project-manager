using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ProjectController;

[Collection("Test collection")]
public class GetAllProjectControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllProjectControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task GetAll_ReturnsAllProjects_WhenSomeProjectsExist()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);
    var createdProjectResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdProjectResponse.Content.ReadFromJsonAsync<Response<ProjectComplex>>();

    // Act
    var response = await _client.GetAsync("/api/project");

    // Assert
    var retrievedProjects = await response.Content.ReadFromJsonAsync<Response<ProjectSimplified[]>>();
    retrievedProjects!.Data.Should().ContainEquivalentOf(createdProject!.Data, o => o.ExcludingMissingMembers());
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

  [Fact]
  public async Task GetAll_ReturnsProjectsByQuery_WhenSuchProjectsExist()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);
    var createdProjectResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdProjectResponse.Content.ReadFromJsonAsync<Response<ProjectComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/project?$filter=managerId eq {createdProject!.Data.ManagerId}");

    // Assert
    var retrievedProjects = await response.Content.ReadFromJsonAsync<Response<ProjectSimplified[]>>();
    retrievedProjects!.Data.Should().ContainEquivalentOf(createdProject!.Data, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
