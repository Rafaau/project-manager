using System.Diagnostics;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class AppointmentService : IAppointmentService
{
  private readonly IRepository<Appointment> _appointmentRepository;
  private readonly IRepository<User> _userRepository;
  private readonly ILoggerAdapter<AppointmentService> _logger;
  public AppointmentService(
    IRepository<Appointment> appointmentRepository,
    IRepository<User> userRepository,
    ILoggerAdapter<AppointmentService> logger)
  {
    _appointmentRepository = appointmentRepository;
    _userRepository = userRepository;
    _logger = logger;
  }

  public async Task<IQueryable<Appointment>> RetrieveAllAppointments()
  {
    _logger.LogInformation("Retrieving all appointments");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var appointments = await _appointmentRepository.ListAsync();

      return appointments.AsQueryable();
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving appointments");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Appointments retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Appointment> CreateAppointment(Appointment request)
  {
    _logger.LogInformation("Creating appointment");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var users = new List<User>();
      foreach (var user in request.Users)
      {
        var userSpec = new UserById(user.Id);
        var userToAdd = await _userRepository.FirstOrDefaultAsync(userSpec);
        users.Add(userToAdd);
      }

      request.Users = users;

      var createdAppointment = await _appointmentRepository.AddAsync(request);
      return createdAppointment;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating appointment");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Appointment created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }
}
