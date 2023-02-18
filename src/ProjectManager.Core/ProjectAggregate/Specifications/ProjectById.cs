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
