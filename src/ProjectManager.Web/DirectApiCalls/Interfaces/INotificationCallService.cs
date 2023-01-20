using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface INotificationCallService
{
  public Task<Response<NotificationComplex[]>> GetByUserId(int userId);
  public Task<Response<NotificationComplex>> CreateNotification(NotificationRequest request);
  public Task<Response<NotificationComplex>> SetNotificationAsSeen(int notificationId);
}
