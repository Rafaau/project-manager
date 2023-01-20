using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface IAppointmentCallService
{
  Task<Response<AppointmentComplex[]>> GetByUserId(int userId);
  Task<Response<AppointmentComplex>> CreateAppointment(AppointmentRequest request);
}
