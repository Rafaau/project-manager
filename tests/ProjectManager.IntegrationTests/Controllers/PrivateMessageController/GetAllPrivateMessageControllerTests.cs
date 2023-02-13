using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.PrivateMessageController;

[Collection("Test collection")]
public class GetAllPrivateMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllPrivateMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task GetAll_ReturnsMessages_WhenSomeMessagesExist()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/privatemessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();

    // Act
    var response = await _client.GetAsync("/api/privatemessage");

    // Assert
    var retrievedMessages = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex[]>>();
    retrievedMessages!.Data.Should().ContainEquivalentOf(createdMessage!.Data, o => o.Excluding(p => p.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsEmptyList_WhenNoMessagesExist()
  {
    // Act
    var response = await _client.GetAsync("/api/privatemessage");

    // Assert
    var retrievedMessages = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex[]>>();
    retrievedMessages!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsMessagesByQuery_WhenSuchMessagesExist()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/privatemessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();
    var data = createdMessage!.Data;

    // Act
    var response = await _client.GetAsync($"/api/privatemessage?$filter=(senderId eq {data.Sender.Id} and receiverId eq {data.Receiver.Id}) or (senderId eq {data.Receiver.Id} and receiverId eq {data.Sender.Id})");

    // Assert
    var retrievedMessages = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex[]>>();
    retrievedMessages!.Data.Should().ContainEquivalentOf(createdMessage!.Data, o => o.Excluding(p => p.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
