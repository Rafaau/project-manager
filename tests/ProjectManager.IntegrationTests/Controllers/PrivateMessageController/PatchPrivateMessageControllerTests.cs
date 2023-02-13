using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.PrivateMessageController;

[Collection("Test collection")]
public class PatchPrivateMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public PatchPrivateMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Patch_ReturnsOk_WhenMessageExist()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/privatemessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();

    // Act
    var response = await _client.PatchAsync($"/api/privatemessage/{createdMessage!.Data.Id}", null);

    // Assert
    var messageResponse = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();
    messageResponse!.Data.Should().BeEquivalentTo(createdMessage.Data, o => o
      .Excluding(p => p.IsSeen)
      .Excluding(p => p.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Patch_ReturnsNotFound_WhenMessageDoesNotExist()
  {
    // Act
    var response = await _client.PatchAsync("/api/privatemessage/1", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
