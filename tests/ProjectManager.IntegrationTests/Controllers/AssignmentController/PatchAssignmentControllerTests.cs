using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentController;

[Collection("Test collection")]
public class PatchAssignmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public PatchAssignmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task MoveToStage_MovesAssignmentToAnotherStage_WhenDataIsValid()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    var stage = _stageGenerator.Generate();
    stage.ProjectId = createdAssignment!.Data.Project.Id;
    var stageResponse = await _client.PostAsJsonAsync("/api/assignmentstage", stage);
    var createdStage = await stageResponse.Content.ReadFromJsonAsync<Response<AssignmentStageComplex>>();
    var expected = createdAssignment.Data;
    expected.AssignmentStage.Id = createdStage!.Data.Id;
    expected.AssignmentStage.Index = createdStage!.Data.Index;

    // Act
    var response = await _client.PatchAsync($"/api/assignment/{createdAssignment!.Data.Id}/{createdStage!.Data.Id}", null);

    // Assert
    var assignmentResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    assignmentResponse!.Data.Should().BeEquivalentTo(expected, o => o.Excluding(a => a.Deadline));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task MoveToStage_ReturnsError_WhenStageDoesNotExist()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();

    // Act
    var response = await _client.PatchAsync($"/api/assignment/{createdAssignment!.Data.Id}/10", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  [Fact]
  public async Task SignUpUser_SignsUpUserToAssignment_WhenDataIsValid()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    var user = _userGenerator.Generate();
    var userResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await userResponse.Content.ReadFromJsonAsync<Response<UserComplex>>();

    // Act
    var response = await _client.PatchAsync($"/api/assignment/SignUp/{createdAssignment!.Data.Id}/{createdUser!.Data.Id}", null);

    // Assert
    var assignmentResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();
    assignmentResponse!.Data.Users.Should().HaveCount(2);
    assignmentResponse!.Data.Users.Should().ContainEquivalentOf(createdUser.Data, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task SignUpUser_ReturnsError_WhenUserDoesNotExist()
  {
    // Arrange
    var assignment = _assignmentGenerator.Generate();
    await assignment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignment", assignment);
    var createdAssignment = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentComplex>>();

    // Act
    var response = await _client.PatchAsync($"/api/assignment/SignUp/{createdAssignment!.Data.Id}/10", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
