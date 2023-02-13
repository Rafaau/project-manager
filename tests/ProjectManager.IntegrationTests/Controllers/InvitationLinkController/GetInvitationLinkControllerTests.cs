using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;

namespace ProjectManager.IntegrationTests.InvitationLinkController;

[Collection("Test collection")]
public class GetInvitationLinkControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetInvitationLinkControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Get_ReturnsInvitationLink_WhenExists()
  {
    // Arrange
    var invitationLink = new InvitationLinkRequest();
    await invitationLink.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/invitationlink", invitationLink);
    var createdLink = await createdResponse.Content.ReadFromJsonAsync<Response<InvitationLinkComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/invitationlink/{createdLink!.Data.Url}");

    // Assert
    var linkResponse = await response.Content.ReadFromJsonAsync<Response<InvitationLinkComplex>>();
    linkResponse!.Data.ProjectId.Should().Be(createdLink.Data.ProjectId);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Get_ReturnsNotFound_WhenInvitationLinkDoesNotExist()
  {
    // Act
    var response = await _client.GetAsync($"/api/invitationlink/{Guid.NewGuid()}");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
