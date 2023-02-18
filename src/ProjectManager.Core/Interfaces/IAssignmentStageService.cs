using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IAssignmentStageService
{
  Task<AssignmentStage> UpdateAssignmentStage(int stageId, string name);
  Task<AssignmentStage> AddAssignmentStage(AssignmentStage request);
  Task<AssignmentStage> DeleteAssignmentStage(int stageId);
}
