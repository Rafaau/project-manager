using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class PrivateMessage : EntityBase, IAggregateRoot
{
  public int SenderId { get; set; }
  public virtual User Sender { get; set; }
  public int ReceiverId { get; set; }
  public virtual User Receiver { get; set; }
  public string Content { get; set; }
  public bool IsSeen { get; set; } = false;
  public DateTime PostDate { get; init; } = DateTime.UtcNow;
}
