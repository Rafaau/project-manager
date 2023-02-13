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
public class UpdateAssignmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public UpdateAssignmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Update_UpdatesAssignment_WhenDataIsValid()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    var assignmentToUpdate = createdAssignment!.Data;
    assignmentToUpdate.Name = "Test";

    // Act
    var response = await _client.PutAsJsonAsync("/api/assignment", assignmentToUpdate);

    // Assert
    var assignmentResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    assignmentResponse!.Data.Should().BeEquivalentTo(assignmentToUpdate);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Update_ReturnsValidationError_WhenDeadlineIsInvalid()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    var assignmentToUpdate = createdAssignment!.Data;
    assignmentToUpdate.Deadline = DateTime.UtcNow.AddDays(-1);

    // Act
    var response = await _client.PutAsJsonAsync("/api/assignment", assignmentToUpdate);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Deadline"][0].Should().Be("Deadline should be set as later than today.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
