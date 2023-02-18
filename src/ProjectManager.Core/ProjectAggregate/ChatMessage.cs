using System.ComponentModel.DataAnnotations.Schema;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;

[Table("ChatMessages")]
public class ChatMessage : EntityBase, IAggregateRoot
{
  public string Content { get; set; } = string.Empty;
  public int UserId { get; set; }
  public virtual User? User { get; set; }
  public int ProjectId { get; set; }
  public virtual Project2? Project { get; set; }
  public DateTime PostDate { get; set; } = DateTime.UtcNow;
  public int ChatChannelId { get; set; }
  public virtual ChatChannel? ChatChannel { get; set; }

  public ChatMessage()
  {
  }
}
