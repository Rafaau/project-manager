using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.PrivateMessageController;

[Collection("Test collection")]
public class GetPrivateMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetPrivateMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Get_ReturnsConversations_WhenSomeExist()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/privatemessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/privatemessage/{createdMessage!.Data.Sender.Id}");

    // Assert
    var retrievedConversations = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex[]>>();
    retrievedConversations!.Data.Should()
      .ContainEquivalentOf(createdMessage.Data, o => o.Excluding(p => p.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Get_ReturnsEmptyList_WhenNoConversationsExist()
  {
    // Act
    var response = await _client.GetAsync($"/api/privatemessage/1");

    // Assert
    var retrievedConversations = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex[]>>();
    retrievedConversations!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);

  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
