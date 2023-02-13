using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ChatMessageController;

[Collection("Test collection")]
public class DeleteChatMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public DeleteChatMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenMessageExist()
  {
    // Arrange
    var message = _chatMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatmessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();

    // Act
    var response = await _client.DeleteAsync($"/api/chatmessage/{createdMessage!.Data.Id}");

    // Assert
    var messageResponse = await response.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();
    messageResponse!.Data.Should().BeEquivalentTo(createdMessage.Data, o => o
      .Excluding(m => m.PostDate)
      .Excluding(m => m.Project)
      .Excluding(m => m.User));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsNotFound_WhenMessageDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync("/api/chatmessage/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
