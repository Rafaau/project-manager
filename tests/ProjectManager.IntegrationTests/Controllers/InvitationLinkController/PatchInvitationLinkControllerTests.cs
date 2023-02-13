using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;

namespace ProjectManager.IntegrationTests.InvitationLinkController;

[Collection("Test collection")]
public class PatchInvitationLinkControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public PatchInvitationLinkControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Patch_ReturnsOk_WhenInvitationLinkExists()
  {
    // Arrange
    var invitationLink = new InvitationLinkRequest();
    await invitationLink.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/invitationlink", invitationLink);
    var createdLink = await createdResponse.Content.ReadFromJsonAsync<Response<InvitationLinkComplex>>();

    // Act
    var response = await _client.PatchAsync($"/api/invitationlink/{createdLink!.Data.Id}", null);

    // Assert
    var linkResponse = await response.Content.ReadFromJsonAsync<Response<InvitationLinkComplex>>();
    linkResponse!.Data.Should().BeEquivalentTo(createdLink.Data, o => o
      .Excluding(i => i.IsUsed)
      .Excluding(i => i.ExpirationTime));
    linkResponse.Data.IsUsed.Should().BeTrue();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Patch_ReturnsNotFound_WhenInvitationLinkDoesNotExist()
  {
    // Act
    var response = await _client.PatchAsync("/api/invitationlink/1", null);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
