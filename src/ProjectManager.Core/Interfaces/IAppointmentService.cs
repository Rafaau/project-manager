using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IAppointmentService
{
    Task<IQueryable<Appointment>> RetrieveAllAppointments();
    Task<Appointment> CreateAppointment(Appointment request);
}
