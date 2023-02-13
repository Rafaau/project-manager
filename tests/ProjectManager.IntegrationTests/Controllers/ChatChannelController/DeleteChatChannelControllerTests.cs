using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ChatChannelController;

[Collection("Test collection")]
public class DeleteChatChannelControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public DeleteChatChannelControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenChatChannelExists()
  {
    // Arrange
    var stage = _chatChannelGenerator.Generate();
    await stage.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatchannel", stage);
    var createdStage = await createdResponse.Content.ReadFromJsonAsync<Response<ChatChannelComplex>>();

    // Act
    var response = await _client.DeleteAsync($"/api/chatchannel/{createdStage!.Data.Id}");

    // Assert
    var stageResponse = await response.Content.ReadFromJsonAsync<Response<ChatChannelComplex>>();
    stageResponse!.Data.Should().BeEquivalentTo(createdStage.Data, o => o.Excluding(c => c.PermissedUsers));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsNotFound_WhenChatChannelDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync("/api/chatchannel/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
