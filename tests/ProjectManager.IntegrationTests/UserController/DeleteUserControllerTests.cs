using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.UserController.UserGenerator;

namespace ProjectManager.IntegrationTests.UserController;
public class DeleteUserControllerTests : IClassFixture<ApiFactory>
{
  private readonly HttpClient _client;

  public DeleteUserControllerTests(ApiFactory factory)
  {
    AppDbContextOptions.IsTesting = true;
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Delete_ReturnsOk_WhenUserExists()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<UserSimplified>();

    // Act
    var response = await _client.DeleteAsync($"/api/user/{createdUser!.Id}");

    // Assert
    var deletedUser = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    deletedUser!.Data.Should().BeEquivalentTo(createdUser);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
  {
    // Act
    var response = await _client.DeleteAsync("/api/user/1");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }
}
