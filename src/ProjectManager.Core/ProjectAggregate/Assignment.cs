using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class Assignment : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public int ProjectId { get; set; }
  public virtual Project2? Project { get; set; }
  public virtual ICollection<User>? Users { get; set; }
  public int AssignmentStageId { get; set; }
  public virtual AssignmentStage AssignmentStage { get; set; }
  public Priority Priority { get; set; }
  public DateTime Deadline { get; set; }

  public Assignment(string name, string description, Project2 project, DateTime deadline, Priority priority) : this(name, description, priority, deadline)
  {
    Project = project;
  }

  public Assignment(string name, string description, Priority priority, DateTime deadline)
  {
    Name = name;
    Description = description;
    Priority = priority;
    Deadline = deadline;
  }
  public Assignment()
  {
  }
}
