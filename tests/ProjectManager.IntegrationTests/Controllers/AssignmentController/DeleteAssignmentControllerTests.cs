using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentController;

[Collection("Test collection")]
public class DeleteAssignmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public DeleteAssignmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenUserExists()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();

    // Act
    var response = await _client.DeleteAsync($"/api/assignment/{createdAssignment!.Data.Id}");

    // Assert
    var deletedAssignment = await response.Content.ReadFromJsonAsync<Response<AssignmentSimplified>>();
    deletedAssignment!.Data.Should().BeEquivalentTo(createdAssignment.Data, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsNotFound_WhenAssignmentDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync("/api/assignment/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
