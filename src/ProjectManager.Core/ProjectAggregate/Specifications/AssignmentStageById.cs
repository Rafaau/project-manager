using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class AssignmentStageById : Specification<AssignmentStage>
{
  public AssignmentStageById(int id)
  {
    Query
      .Where(stage => stage.Id == id);
  }
}
