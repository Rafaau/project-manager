using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentStageController;

[Collection("Test collection")]
public class DeleteAssignmentStageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public DeleteAssignmentStageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenStageExists()
  {
    // Arrange
    var stage = _stageGenerator.Generate();
    await stage.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignmentstage", stage);
    var createdStage = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentStageComplex>>();

    // Act
    var response = await _client.DeleteAsync($"/api/assignmentstage/{createdStage!.Data.Id}");

    // Assert
    var stageResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentStageSimplified>>();
    stageResponse!.Data.Should().BeEquivalentTo(createdStage.Data, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsError_WhenStageDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync("/api/assignmentstage/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
