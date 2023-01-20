using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
