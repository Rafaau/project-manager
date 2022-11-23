using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class ProjectById : Specification<Project2>, ISingleResultSpecification 
{
  public ProjectById(int projectId)
  {
    Query
      .Where(project => project.Id == projectId);
  }
}
