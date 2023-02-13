using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ProjectController;

[Collection("Test collection")]
public class UpdateProjectControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public UpdateProjectControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Update_ReturnsOk_WhenDataIsValid()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdResponse.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    var projectToUpdate = createdProject!.Data;
    projectToUpdate.Name = "Test";

    // Act
    var response = await _client.PutAsJsonAsync("/api/project", projectToUpdate);

    // Assert
    var projectResponse = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    projectResponse!.Data.Should().BeEquivalentTo(projectToUpdate);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Update_ReturnsNotFound_WhenProjectDoesNotExist()
  {
    // Act
    var response = await _client.PutAsJsonAsync("/api/project", new ProjectComplex() { Name = "Test" });

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task Update_ReturnsValidationError_WhenNameIsEmpty()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/project", project);
    var createdProject = await createdResponse.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    var projectToUpdate = createdProject!.Data;
    projectToUpdate.Name = string.Empty;

    // Act
    var response = await _client.PutAsJsonAsync("/api/project", projectToUpdate);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Name"][0].Should().Be("'Name' must not be empty.");
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
