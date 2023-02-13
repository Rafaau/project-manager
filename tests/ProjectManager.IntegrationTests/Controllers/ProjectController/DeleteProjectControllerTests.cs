using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ProjectController;

[Collection("Test collection")]
public class DeleteProjectControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public DeleteProjectControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenProjectExists()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdResponse.Content.ReadFromJsonAsync<Response<ProjectComplex>>();

    // Act
    var response = await _client.DeleteAsync($"/api/project/{createdProject!.Data.Id}");

    // Assert
    var projectResponse = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    projectResponse!.Data.Should().BeEquivalentTo(createdProject, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsNotFound_WhenProjectDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync($"/api/project/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
