using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class NotificationById : Specification<Notification>
{
  public NotificationById(int notificationId)
  {
    Query
      .Where(n => n.Id == notificationId);
  }
}
