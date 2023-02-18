using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class AssignmentById: Specification<Assignment>
{
  public AssignmentById(int assignmentId)
  {
    Query
      .Where(assignment => assignment.Id == assignmentId);
  }
}
