using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class AppointmentCallService : ServiceBase, IAppointmentCallService
{
  public AppointmentCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<AppointmentComplex[]>> GetByUserId(int userId)
  {
    return await HttpClient.GetResponse<AppointmentComplex[]>($"/api/appointment?$filter=users/any(u: u/id eq {userId})");
  }

  public async Task<Response<AppointmentComplex>> CreateAppointment(AppointmentRequest request)
  {
    return await HttpClient.Post<AppointmentRequest, AppointmentComplex>("/api/appointment", request);
  }
}
