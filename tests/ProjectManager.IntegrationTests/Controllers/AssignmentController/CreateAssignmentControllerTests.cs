using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentController;

[Collection("Test collection")]
public class CreateAssignmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateAssignmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreateAssignment_WhenDataIsValid()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/assignment", assignment);

    // Assert
    var assignmentResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    assignmentResponse!.Data.Should().BeEquivalentTo(assignment, o => o
      .Excluding(a => a.ProjectId)
      .Excluding(a => a.AssignmentStageId));
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be($"http://localhost/api/Assignment");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenDeadlineIsInvalid()
  {
    // Arrange
    var assignment = _assignmentGenerator.Clone()
      .RuleFor(x => x.Deadline, DateTime.UtcNow.AddDays(-1)).Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/assignment", assignment);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Deadline"][0].Should().Be("Deadline should be set as later than today.");
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
