using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.NotificationController;

[Collection("Test collection")]
public class CreateNotificationControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateNotificationControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesNotification_WhenDataIsValid()
  {
    // Arrange
    var notification = _notificationGenerator.Generate();
    await notification.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/notification", notification);

    // Assert
    var notificationResponse = await response.Content.ReadFromJsonAsync<Response<NotificationComplex>>();
    notificationResponse!.Data.Should().BeEquivalentTo(notification, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be("http://localhost/api/Notification");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenContentIsEmpty()
  {
    // Arrange
    var notification = _notificationGenerator.Clone()
      .RuleFor(x => x.Content, string.Empty).Generate();
    await notification.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/notification", notification);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Content"][0].Should().Be("'Content' must not be empty.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Create_ReturnsError_WhenUserDoesNotExist()
  {
    // Arrange
    var notification = _notificationGenerator.Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/notification", notification);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
