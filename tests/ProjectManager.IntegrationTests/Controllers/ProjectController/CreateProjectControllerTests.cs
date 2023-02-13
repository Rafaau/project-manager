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
public class CreateProjectControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateProjectControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesProject_WhenDataIsValid()
  {
    // Arrange
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/project", project);

    // Assert
    var projectResponse = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    projectResponse!.Data.Should().BeEquivalentTo(project, o => o
      .ExcludingMissingMembers()
      .Excluding(p => p.AssignmentStages)
      .Excluding(p => p.ChatChannels));
    projectResponse.Data.ChatChannels
      .Should().HaveCount(1);
    projectResponse.Data.AssignmentStages
      .Should().HaveCount(3);
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be($"http://localhost/api/Project/{projectResponse.Data.Id}");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenNameIsEmpty()
  {
    // Arrange
    var project = _projectGenerator.Clone()
      .RuleFor(x => x.Name, string.Empty).Generate();
    await project.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/project", project);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Name"][0].Should().Be("'Name' must not be empty.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Create_ReturnsInternalServerError_WhenMissingManager()
  {
    // Arrange
    var project = _projectGenerator.Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/project", project);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
