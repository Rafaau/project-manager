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
public class UpdatePrivateMessageControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public UpdatePrivateMessageControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Update_ReturnsOk_WhenDataIsValid()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/privatemessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();
    var messageToUpdate = createdMessage!.Data;
    messageToUpdate.Content = "Test";

    // Act
    var response = await _client.PutAsJsonAsync("/api/privatemessage", messageToUpdate);

    // Assert
    var messageResponse = await response.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();
    messageResponse!.Data.Should().BeEquivalentTo(messageToUpdate, o => o.Excluding(p => p.PostDate));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Update_ReturnsValidationError_WhenContentIsEmpty()
  {
    // Arrange
    var message = _privateMessageGenerator.Generate();
    await message.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/privatemessage", message);
    var createdMessage = await createdResponse.Content.ReadFromJsonAsync<Response<PrivateMessageComplex>>();
    var messageToUpdate = createdMessage!.Data;
    messageToUpdate.Content = string.Empty;

    // Act
    var response = await _client.PutAsJsonAsync("/api/privatemessage", messageToUpdate);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Content"][0].Should().Be("'Content' must not be empty.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Update_ReturnsNotFound_WhenMessageDoesNotExist()
  {
    // Act
    var response = await _client.PutAsJsonAsync("/api/privatemessage", new PrivateMessageSimplified() { Content = "Test" });

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
