using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AssignmentStageController;

[Collection("Test collection")]
public class PatchAssignmentStageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public PatchAssignmentStageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Patch_EditsStageName_WhenStageExists()
  {
    // Arrange
    var stage = _stageGenerator.Generate();
    await stage.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/assignmentstage", stage);
    var createdStage = await createdResponse.Content.ReadFromJsonAsync<Response<AssignmentStageComplex>>();
    var expected = createdStage!.Data;
    expected.Name = "Test";

    // Act
    var response = await _client.PatchAsync($"/api/assignmentstage/{createdStage.Data.Id}/Test", null);

    // Assert
    var stageResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentStageComplex>>();
    stageResponse!.Data.Should().BeEquivalentTo(expected);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Patch_ReturnsNotFound_WhenStageDoesNotExist()
  {
    // Act
    var response = await _client.PatchAsync("/api/assignmentstage/1/Test", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
