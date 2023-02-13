using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentController;

[Collection("Test collection")]
public class GetAllAssignmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllAssignmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task GetAll_ReturnsAllAssignments_WhenSomeAssignmentsExist()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var assignmentResponse = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();

    // Act
    var response = await _client.GetAsync("/api/assignment");

    // Assert
    var retrievedAssignments = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex[]>>();
    retrievedAssignments!.Data.Should().ContainEquivalentOf(assignmentResponse!.Data, o => o.Excluding(a => a.Deadline));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsAssignmentsByQuery_WhenSuchAssignmentsExist()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var assignmentResponse = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/assignment?$filter=users/any(u: u/id eq {assignmentResponse!.Data.Users.First().Id})");

    // Assert
    var retrievedAssignments = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex[]>>();
    retrievedAssignments!.Data.Should().ContainEquivalentOf(assignmentResponse!.Data, o => o.Excluding(a => a.Deadline));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsEmptyResult_WhenNoAssignmentsExist()
  {
    // Act
    var response = await _client.GetAsync("/api/assignment");

    // Assert
    var retrievedAssignments = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex[]>>();
    retrievedAssignments!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;
  public Task DisposeAsync() => _resetDatabase();

}
