using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.ChatChannelController;

[Collection("Test collection")]
public class CreateChatChannelControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateChatChannelControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesChatChannel_WhenDataIsValid()
  {
    // Arrange
    var channel = _chatChannelGenerator.Generate();
    await channel.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/chatchannel", channel);

    // Assert
    var channelResponse = await response.Content.ReadFromJsonAsync<Response<ChatChannelComplex>>();
    channelResponse!.Data.Should().BeEquivalentTo(channel, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.Should()
      .Be($"http://localhost/api/ChatChannel");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenNameIsEmpty()
  {
    // Arrange
    var channel = _chatChannelGenerator.Clone()
      .RuleFor(x => x.Name, string.Empty).Generate();
    await channel.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/chatchannel", channel);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Name"][0].Should().Be("'Name' must not be empty.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Create_ReturnsError_WhenProjectDoesNotExist()
  {
    // Arrange
    var channel = _chatChannelGenerator.Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/chatchannel", channel);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
