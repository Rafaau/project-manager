using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IAppointmentService
{
    Task<IQueryable<Appointment>> RetrieveAllAppointments();
    Task<Appointment> CreateAppointment(Appointment request);
}
