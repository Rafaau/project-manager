using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ProjectController;

[Collection("Test collection")]
public class GetProjectControllerTest : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetProjectControllerTest(ApiFactory factory)
  {
    _client = factory.CreateClient();
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Get_ReturnsProject_WhenProjectExists()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);
    var createdProjectResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdProjectResponse.Content.ReadFromJsonAsync<Response<ProjectSimplified>>();

    // Act
    var response = await _client.GetAsync($"/api/project/{createdProject!.Data.Id}");

    // Assert
    var retrievedProject = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    retrievedProject!.Data.Should().BeEquivalentTo(createdProject.Data);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Get_ReturnsNotFound_WhenProjectDoesNotExist()
  {
    // Act
    var response = await _client.GetAsync("/api/project/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
