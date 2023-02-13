using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AppointmentController;

[Collection("Test collection")]
public class CreateAppointmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public CreateAppointmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }

  [Fact]
  public async Task Create_CreatesAppointment_WhenDataIsValid()
  {
    // Arrange
    var appointment = _appointmentGenerator.Generate();
    await appointment.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/appointment", appointment);

    // Assert
    var appointmentResponse = await response.Content.ReadFromJsonAsync<Response<AppointmentComplex>>();
    appointmentResponse!.Data.Should().BeEquivalentTo(appointment);
    response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    response.Headers.Location!.ToString().Should()
      .Be($"http://localhost/api/Appointment");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenDateIsPast()
  {
    // Arrange
    var invalidDate = DateTime.UtcNow.AddDays(-1);
    var appointment = _appointmentGenerator.Clone()
      .RuleFor(x => x.Date, invalidDate).Generate();
    await appointment.IncludeRelationships(_client);

    // Act
    var response = await _client.PostAsJsonAsync("/api/appointment", appointment);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Date"][0].Should().Be("Appointment must be scheduled with today or future date.");
  }

  [Fact]
  public async Task Create_ReturnsValidationError_WhenNoUsersAssigned()
  {
    // Arrange
    var appointment = _appointmentGenerator.Generate();

    // Act
    var response = await _client.PostAsJsonAsync("/api/appointment", appointment);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    error!.Status.Should().Be(400);
    error.Title.Should().Be("One or more validation errors occurred.");
    error.Errors["Users"][0].Should().Be("Appointment must be assigned to at least one user.");
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
