using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class ProjectsByManagerId : Specification<Project2>
{
  public ProjectsByManagerId(int managerId)
  {
    Query
      .Where(project => project.ManagerId == managerId);
  }
}
