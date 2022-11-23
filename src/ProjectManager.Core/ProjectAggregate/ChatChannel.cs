using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class ChatChannel : EntityBase, IAggregateRoot
{
  public string Name { get; set; }
  public int ProjectId { get; set; }
  public virtual Project2 Project { get; set; }
  public virtual ICollection<User>? PermissedUsers { get; set; }
  public virtual ICollection<Message>? Messages { get; set; }

  public ChatChannel()
  {
  }
}
