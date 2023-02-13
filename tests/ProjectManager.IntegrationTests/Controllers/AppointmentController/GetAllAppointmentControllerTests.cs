using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using Xunit;
using static ProjectManager.IntegrationTests.FakerGenerator;

namespace ProjectManager.IntegrationTests.AppointmentController;

[Collection("Test collection")]
public class GetAllAppointmentControllerTests : IAsyncLifetime
{
  private readonly HttpClient _client;
  private readonly Func<Task> _resetDatabase;

  public GetAllAppointmentControllerTests(ApiFactory factory)
  {
    _client = factory.HttpClient;
    _resetDatabase = factory.ResetDatabaseAsync;
  }


  [Fact]
  public async Task GetAll_ReturnsAllAppointments_WhenAppointmentsExist()
  {
    // Arrange
    var appointment = _appointmentGenerator.Generate();
    await appointment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/appointment", appointment);
    var createdAppointment = await createdResponse.Content.ReadFromJsonAsync<Response<AppointmentComplex>>();

    // Act
    var response = await _client.GetAsync("/api/appointment");

    // Assert
    var retrievedAppointments = await response.Content.ReadFromJsonAsync<Response<AppointmentComplex[]>>();
    retrievedAppointments!.Data.Should().ContainEquivalentOf(createdAppointment!.Data, o => o.Excluding(a => a.Id).Excluding(a => a.Date));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsAppointmentsByQuery_WhenSuchExist()
  {
    // Arrange
    var appointment = _appointmentGenerator.Generate();
    await appointment.IncludeRelationships(_client);
    var createdResponse = await _client.PostAsJsonAsync("/api/appointment", appointment);
    var createdAppointment = await createdResponse.Content.ReadFromJsonAsync<Response<AppointmentComplex>>();

    // Act
    var response = await _client.GetAsync($"/api/appointment?$filter=users/any(u: u/id eq {createdAppointment!.Data.Users.First().Id})");

    // Assert
    var retrievedAppointments = await response.Content.ReadFromJsonAsync<Response<AppointmentComplex[]>>();
    retrievedAppointments!.Data.Should().ContainEquivalentOf(createdAppointment!.Data, o => o.Excluding(a => a.Id).Excluding(a => a.Date));
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task GetAll_ReturnsEmptyResult_WhenNoAppointmentsExist()
  {
    // Act
    var response = await _client.GetAsync("/api/appointment");

    // Assert
    var retrievedAppointments = await response.Content.ReadFromJsonAsync<Response<AppointmentComplex[]>>();
    retrievedAppointments!.Data.Should().BeEmpty();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  public Task InitializeAsync() => Task.CompletedTask;

  public Task DisposeAsync() => _resetDatabase();
}
