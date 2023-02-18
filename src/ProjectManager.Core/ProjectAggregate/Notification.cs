using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class Notification : EntityBase, IAggregateRoot
{
  public int UserId { get; set; }
  public virtual User User { get; set; }
  public string Content { get; set; } = string.Empty;
  public bool IsSeen { get; set; } = false;
  public DateTime Date { get; init; } = DateTime.UtcNow;
}
