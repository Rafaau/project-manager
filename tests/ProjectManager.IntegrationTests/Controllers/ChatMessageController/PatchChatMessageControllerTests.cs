using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ChatMessageController;

[Collection("Test collection")]
public class PatchChatMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public PatchChatMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Patch_ReturnsOk_WhenDataIsValid()
  {
    // Arrange
    var message = _chatMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatmessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();
    var messageToPatch = createdMessage!.Data;
    messageToPatch.Content = "Test";

    // Act
    var response = await _client.PatchAsync($"/api/chatmessage/{messageToPatch.Id}/{messageToPatch.Content}", null);

    // Assert
    var messageResponse = await response.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();
    messageResponse!.Data.Should().BeEquivalentTo(messageToPatch, o => o.Excluding(m => m.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Patch_ReturnsNotFound_WhenMessageDoesNotExist()
  {
    // Act
    var response = await _client.PatchAsync("/api/chatmessage/1/Test", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task Patch_ReturnsBadRequest_WhenContentIsEmpty()
  {
    // Arrange
    var message = _chatMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatmessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();
    var messageToPatch = createdMessage!.Data;
    messageToPatch.Content = string.Empty;

    // Act
    var response = await _client.PatchAsync($"/api/chatmessage/{messageToPatch.Id}/{messageToPatch.Content}", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
