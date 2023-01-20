using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class Project2 : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = string.Empty;
  public int ManagerId { get; set; }
  public virtual User Manager { get; set; }
  public virtual ICollection<User>? Users { get; set; }
  public virtual ICollection<Assignment>? Assignments { get; set; }
  public virtual ICollection<ChatMessage>? Messages { get; set; }
  public virtual ICollection<AssignmentStage> AssignmentStages { get; set; } = new List<AssignmentStage>();
  public virtual ICollection<ChatChannel> ChatChannels { get; set; } = new List<ChatChannel>();
  public virtual ICollection<InvitationLink>? InvitationLinks { get; set; } = new List<InvitationLink>();
  public Project2(string name, User manager) : this(name)
  {   
    Manager = manager;
  }

  public Project2()
  {
  }

  // To avoid EF bind exception
  private Project2(string name)
  {
    Name = name;
  }

}
