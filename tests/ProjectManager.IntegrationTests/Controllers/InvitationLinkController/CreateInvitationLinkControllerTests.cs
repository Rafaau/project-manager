using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;

namespace ProjectManager.IntegrationTests.InvitationLinkController;

[Collection("Test collection")]
public class CreateInvitationLinkControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateInvitationLinkControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesInvitationLink_WhenProjectExists()
  {
    // Arrange
    var invitationLink = new InvitationLinkRequest();
    await invitationLink.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/invitationlink", invitationLink);

    // Assert
    var invitationLinkResponse = await response.Content.ReadFromJsonAsync<Response<InvitationLinkComplex>>();
    invitationLinkResponse!.Data.ProjectId.Should().Be(invitationLink.ProjectId);
    invitationLinkResponse.Data.IsUsed.Should().BeFalse();
    Guid.TryParse(invitationLinkResponse.Data.Url, out _).Should().BeTrue();
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString()
      .Should().Be("http://localhost/api/InvitationLink");
  }

  [Fact]
  public async Task Create_ReturnsError_WhenProjectDoesNotExist()
  {
    // Arrange
    var invitationLink = new InvitationLinkRequest()
    {
      ProjectId = 1,
    };

    // Act
    var response = await _client.PostAsJsonAsync("/api/invitationlink", invitationLink);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
