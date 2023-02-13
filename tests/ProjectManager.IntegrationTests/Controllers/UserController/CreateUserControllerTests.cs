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
public class CreateUserControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateUserControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesUser_WhenDataIsValid()
  {
    // Arrange
    var user = _userGenerator.Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/user", user);

    // Assert
    var userResponse = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    userResponse!.Data.Should().BeEquivalentTo(user, o => o.Excluding(u => u.Id));
    response.StatusCode.Should().Be(HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be($"http://localhost/api/User/{userResponse!.Data.Email}");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenEmailIsInvalid()
  {
    // Arrange
    const string invalidEmail = "invalid@em";
    var user = _userGenerator.Clone()
      .RuleFor(x => x.Email, invalidEmail).Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/user", user);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Email"][0].Should().Be($"{invalidEmail} is not a valid email address.");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenPasswordIsInvalid()
  {
    // Arrange
    const string invalidPassword = "passw";
    var user = _userGenerator.Clone()
      .RuleFor(x => x.Password, invalidPassword).Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/user", user);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Password"][0].Should().Be($"Password must have at least 6 letters.");
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public async Task DisposeAsync() => _resetDatabase();
}
