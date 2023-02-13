using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ChatMessageController;

[Collection("Test collection")]
public class GetAllChatMessagesControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllChatMessagesControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task GetAll_ReturnsChatMessages_WhenSomeMessagesExist()
  {
    // Arrange
    var message = _chatMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatmessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();

    // Act
    var response = await _client.GetAsync("/api/chatmessage");

    // Assert
    var messagesResponse = await response.Content.ReadFromJsonAsync<Response<ChatMessageComplex[]>>();
    messagesResponse!.Data.Should().ContainEquivalentOf(createdMessage!.Data, o => o.Excluding(c => c.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsEmptyList_WhenNoChatMessagesExist()
  {
    // Act
    var response = await _client.GetAsync("/api/chatmessage");

    // Assert
    var messagesResponse = await response.Content.ReadFromJsonAsync<Response<ChatMessageComplex[]>>();
    messagesResponse!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsMessagesByQuery_WhenSuchMessagesExist()
  {
    // Arrange
    var message = _chatMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatmessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<ChatMessageComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/chatmessage?$filter=projectId eq {createdMessage!.Data.Project.Id}");

    // Assert
    var messagesResponse = await response.Content.ReadFromJsonAsync<Response<ChatMessageComplex[]>>();
    messagesResponse!.Data.Should().ContainEquivalentOf(createdMessage.Data, o => o.Excluding(c => c.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
