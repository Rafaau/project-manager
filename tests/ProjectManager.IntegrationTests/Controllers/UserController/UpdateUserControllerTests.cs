using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.UserController;

[Collection("Test collection")]
public class UpdateUserControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public UpdateUserControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Update_UpdatesUser_WhenDataIsValid()
  {
    // Arrange
    var user = _userGenerator.Generate();
    var createdResponse = await _client.PostAsJsonAsync("/api/user", user);
    var createdUser = await createdResponse.Content.ReadFromJsonAsync<Response<UserSimplified>>();

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

  public async Task DisposeAsync() => _resetDatabase();
}
