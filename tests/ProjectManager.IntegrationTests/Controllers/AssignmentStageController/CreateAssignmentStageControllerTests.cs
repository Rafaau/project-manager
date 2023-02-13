using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentStageController;

[Collection("Test collection")]
public class CreateAssignmentStageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateAssignmentStageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesAssignmentStage_WhenDataIsValid()
  {
    // Arrange
    var stage = _stageGenerator.Generate();
    await stage.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/assignmentstage", stage);

    // Assert
    var stageResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentStageComplex>>();
    stageResponse!.Data.Should().BeEquivalentTo(stage, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be($"http://localhost/api/AssignmentStage");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenDataIsInvalid()
  {
    // Arrange
    var stage = _stageGenerator.Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/assignmentstage", stage);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["ProjectId"][0].Should().Be("'Project Id' must not be empty.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
