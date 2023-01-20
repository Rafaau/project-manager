using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class NotificationCallService : ServiceBase, INotificationCallService
{
  public NotificationCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<NotificationComplex[]>> GetByUserId(int userId)
  {
    return await HttpClient.GetResponse<NotificationComplex[]>($"/api/notification?$filter=userId eq {userId}");
  }

  public async Task<Response<NotificationComplex>> CreateNotification(NotificationRequest request)
  {
    return await HttpClient.Post<NotificationRequest, NotificationComplex>("/api/notification", request);
  }

  public async Task<Response<NotificationComplex>> SetNotificationAsSeen(int notificationId)
  {
    return await HttpClient.Patch<NotificationComplex>($"/api/notification/{notificationId}");
  }
}
