using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.NotificationController;

[Collection("Test collection")]
public class GetAllNotificationControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllNotificationControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task GetAll_ReturnsNotifications_WhenSomeNotificationsExist()
  {
    // Arrange
    var notification = _notificationGenerator.Generate();
    await notification.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/notification", notification);
    var createdNotification = await createdResponse.Content.ReadFromJsonAsync<Response<NotificationComplex>>();

    // Act
    var response = await _client.GetAsync("/api/notification");

    // Assert
    var notificationsResponse = await response.Content.ReadFromJsonAsync<Response<NotificationComplex[]>>();
    notificationsResponse!.Data.Should().ContainEquivalentOf(createdNotification, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ShouldReturnEmptyList_WhenNoNotificationsExist()
  {
    // Act
    var response = await _client.GetAsync("/api/notification");

    // Assert
    var notificationsResponse = await response.Content.ReadFromJsonAsync<Response<NotificationComplex[]>>();
    notificationsResponse!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ShouldReturnNotificationsByQuery_WhenSuchNotificationsExist()
  {
    // Arrange
    var notification = _notificationGenerator.Generate();
    await notification.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/notification", notification);
    var createdNotification = await createdResponse.Content.ReadFromJsonAsync<Response<NotificationComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/notification?$filter=userId eq {createdNotification!.Data.User.Id}");

    // Assert
    var notificationsResponse = await response.Content.ReadFromJsonAsync<Response<NotificationComplex[]>>();
    notificationsResponse!.Data.Should().ContainEquivalentOf(createdNotification, o => o.ExcludingMissingMembers());
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
