using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface INotificationService
{
  Task<IQueryable<Notification>> RetrieveAllNotifications();
  Task<Notification> CreateNotification(Notification request);
  Task<Notification> SetNotificationAsSeen(int notificationId);
}
