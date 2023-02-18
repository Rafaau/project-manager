using FluentAssertions;
using Npgsql;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Services;
using ProjectManager.SharedKernel.Interfaces;
using Xunit;
using static ProjectManager.UnitTests.FakesFactory;

namespace ProjectManager.UnitTests.Core.Services;
public class AppointmentServiceTests
{
  private readonly AppointmentService _sut;
  private readonly IRepository<Appointment> _appointmentRepository = Substitute.For<IRepository<Appointment>>();
  private readonly IRepository<User> _userRepository = Substitute.For<IRepository<User>>();
  private ILoggerAdapter<AppointmentService> _logger = Substitute.For<ILoggerAdapter<AppointmentService>>();

  public AppointmentServiceTests()
  {
    _sut = new AppointmentService(_appointmentRepository, _userRepository, _logger);
  }

  #region RetrieveAll
  [Fact]
  public async Task RetrieveAllAppointments_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _appointmentRepository.ListAsync()
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.RetrieveAllAppointments();

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.LogError(Arg.Is(exception), Arg.Is("Something went wrong while retrieving appointments"));
  }

  [Fact]
  public async Task RetrieveAllAppointments_ShouldReturnEmptyList_WhenNoAppointmentsExist()
  {
    // Arrange
    _appointmentRepository.ListAsync()
      .Returns(new List<Appointment>());

    // Act
    var result = await _sut.RetrieveAllAppointments();

    // Assert
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task RetrieveAllAppointments_ShouldReturnAppointments_WhenSomeAppointmentsExist()
  {
    // Arrange
    var appointments = FakeAppointmentsList();
    _appointmentRepository.ListAsync()
      .Returns(appointments);

    // Act
    var result = await _sut.RetrieveAllAppointments();

    // Assert
    result.Should().BeEquivalentTo(appointments);
  }

  [Fact]
  public async Task RetrieveAllAppointments_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    _appointmentRepository.ListAsync()
      .Returns(FakeAppointmentsList());

    // Act
    await _sut.RetrieveAllAppointments();

    // Assert
    _logger.LogInformation(Arg.Is("Retrieving all appointments"));
    _logger.LogInformation(Arg.Is("Appointments retrieved in {0}ms"), Arg.Any<long>());
  }
  #endregion

  #region Create
  [Fact]
  public async Task CreateAppointment_ShouldLogMessageAndException_WhenExceptionIsThrown()
  {
    // Arrange
    var exception = new NpgsqlException("Something went wrong");
    _appointmentRepository.AddAsync(Arg.Any<Appointment>())
      .Throws(exception);

    // Act
    var requestAction = async () => await _sut.CreateAppointment(FakeAppointment());

    // Assert
    await requestAction.Should()
      .ThrowAsync<NpgsqlException>().WithMessage(exception.Message);
    _logger.Received(1)
      .LogError(Arg.Is(exception), Arg.Is("Something went wrong while creating appointment"));
  }

  [Fact]
  public async Task CreateAppointment_ShouldReturnObject_WhenSucceeded()
  {
    // Arrange
    var appointment = FakeAppointment();
    _appointmentRepository.AddAsync(Arg.Any<Appointment>())
      .Returns(appointment);

    // Act
    var result = await _sut.CreateAppointment(appointment);

    // Assert
    result.Should().BeEquivalentTo(appointment);
  }

  [Fact]
  public async Task CreateAppointment_ShouldLogMessages_WhenInvoked()
  {
    // Arrange
    var appointment = FakeAppointment();
    _appointmentRepository.AddAsync(Arg.Any<Appointment>())
      .Returns(appointment);

    // Act
    await _sut.CreateAppointment(appointment);

    // Assert
    _logger.LogInformation(Arg.Is("Creating appointment"));
    _logger.LogInformation(Arg.Is("Appointment created in {0}ms"), Arg.Any<long>());
  }
  #endregion
}
