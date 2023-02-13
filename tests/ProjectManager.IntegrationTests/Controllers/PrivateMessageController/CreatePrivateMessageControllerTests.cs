using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.PrivateMessageController;

[Collection("Test collection")]
public class CreatePrivateMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreatePrivateMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesPrivateMessage_WhenDataIsValid()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/privatemessage", message);

    // Assert
    var messageResponse = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();
    messageResponse!.Data.Should().BeEquivalentTo(message, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be("http://localhost/api/PrivateMessage");
  }

  [Fact]
  public async Task Create_ShouldReturnValidationError_WhenContentIsEmpty()
  {
    // Arrange
    var message = _privateMessageGenerator.Clone()
      .RuleFor(x => x.Content, string.Empty).Generate();
    await message.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/privatemessage", message);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Content"][0].Should().Be("'Content' must not be empty.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Create_ShouldReturnError_WhenReceiverMissing()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    message.ReceiverId = 0;

    // Act
    var response = await _client.PostAsJsonAsync("/api/privatemessage", message);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  [Fact]
  public async Task Create_ShouldReturnError_WhenSenderMissing()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    message.SenderId = 0;

    // Act
    var response = await _client.PostAsJsonAsync("/api/privatemessage", message);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
