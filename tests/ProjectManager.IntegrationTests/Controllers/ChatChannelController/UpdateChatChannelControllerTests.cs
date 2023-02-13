using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ChatChannelController;

[Collection("Test collection")]
public class UpdateChatChannelControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public UpdateChatChannelControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Update_ReturnsOk_WhenDataIsValid()
  {
    // Arrange
    var channel = _chatChannelGenerator.Generate();
    await channel.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/chatchannel", channel);
    var createdChannel = await createdResponse.Content.ReadFromJsonAsync<Response<ChatChannelComplex>>();
    var user = _userGenerator.Generate();
    var userResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await userResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    var channelToUpdate = createdChannel!.Data;
    channelToUpdate.PermissedUsers = new List<UserSimplified>()
    {
      channelToUpdate.PermissedUsers!.First(),
      createdUser!.Data
    }.ToArray();

    // Act
    var response = await _client.PutAsJsonAsync("/api/chatchannel", channelToUpdate);

    // Assert
    var channelResponse = await response.Content.ReadFromJsonAsync<Response<ChatChannelComplex>>();
    channelResponse!.Data.PermissedUsers.Should().HaveCount(2);
    channelResponse.Data.Should().BeEquivalentTo(channelToUpdate);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Update_ReturnsNotFound_WhenChannelDoesNotExist()
  {
    // Act
    var response = await _client.PutAsJsonAsync("/api/chatchannel", new ChatChannelComplex());

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
