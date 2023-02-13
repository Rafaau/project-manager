using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.NotificationController;

[Collection("Test collection")]
public class PatchNotificationControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public PatchNotificationControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task SetAsSeen_ReturnsOk_WhenNotificationExist()
  {
    // Arrange
    var notification = _notificationGenerator.Generate();
    await notification.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/notification", notification);
    var createdNotification = await createdResponse.Content.ReadFromJsonAsync<Response<NotificationComplex>>();

    // Act
    var response = await _client.PatchAsync($"/api/notification/{createdNotification!.Data.Id}", null);

    // Assert
    var notificationResponse = await response.Content.ReadFromJsonAsync<Response<NotificationComplex>>();
    notificationResponse!.Data.Should().BeEquivalentTo(createdNotification.Data, o => o
      .Excluding(n => n.IsSeen)
      .Excluding(n => n.Date));
    notificationResponse.Data.IsSeen.Should().BeTrue();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task SetAsSeen_ReturnsNotFound_WhenNotificationDoesNotExist()
  {
    // Act
    var response = await _client.PatchAsync("/api/notification/1", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
