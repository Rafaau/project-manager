using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class AssignmentStage : EntityBase, IAggregateRoot
{
  public string Name { get; set; } = string.Empty;
  public int Index { get; set; }
  public virtual ICollection<Assignment>? Assignments { get; set; }
  public int ProjectId { get; set; }
  public virtual Project2 Project { get; set; }

  public AssignmentStage(string name, int index, int projectId)
  {
    Name = name;
    Index = index;
    ProjectId = projectId;
  }

  public AssignmentStage()
  {
  }
}
