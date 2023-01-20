using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class InvitationLink : EntityBase, IAggregateRoot
{
  public string Url { get; set; } = string.Empty;
  public int ProjectId { get; set; }
  public virtual Project2 Project { get; set; }
  public bool IsUsed { get; set; } = false;
  public DateTime ExpirationTime { get; set; } = DateTime.UtcNow.AddDays(3);
}
