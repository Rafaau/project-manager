using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Infrastructure.Data.Config;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.UserController.UserGenerator;

namespace ProjectManager.IntegrationTests.UserController;
public class UpdateUserControllerTests : IClassFixture<ApiFactory>, IAsyncLifetime
{
  private readonly HttpClient _client;
  private List<int> _idsToDelete = new();

  public UpdateUserControllerTests(ApiFactory factory)
  {
    AppDbContextOptions.IsTesting = true;
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task Update_UpdatesUser_WhenDataIsValid()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    _idsToDelete.Add(createdUser!.Data.Id);

    user = _userGenerator.Clone()
      .RuleFor(x => x.Id, createdUser!.Data.Id).Generate();

    // Act
    var response = await _client.PutAsJsonAsync($"/api/user", user);

    // Assert
    var userResponse = await response.Content.ReadFromJsonAsync<Response<UserComplex>>();
    userResponse!.Data.Should().BeEquivalentTo(user);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task Update_ReturnsValidationError_WhenEmailIsInvalid()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<UserSimplified>();
    _idsToDelete.Add(createdUser!.Id);

    const string invalidEmail = "invalid@em";
    user = _userGenerator.Clone()
      .RuleFor(x => x.Id, createdUser!.Id)
      .RuleFor(x => x.Email, invalidEmail).Generate();

    // Act
    var response = await _client.PutAsJsonAsync($"/api/user", user);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Update_ReturnsValidationError_WhenPasswordIsInvalid()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<UserSimplified>();
    _idsToDelete.Add(createdUser!.Id);

    user = _userGenerator.Clone()
      .RuleFor(x => x.Id, createdUser!.Id)
      .RuleFor(x => x.Password, "passw").Generate();

    // Act
    var response = await _client.PutAsJsonAsync($"/api/user", user);

    // Assert
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Password"][0].Should().Be($"Password must have at least 6 letters.");
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public async Task DisposeAsync()
  {
    foreach (var id in _idsToDelete)
    {
      await _client.DeleteAsync($"/api/user/{id}");
    }
  }
}
