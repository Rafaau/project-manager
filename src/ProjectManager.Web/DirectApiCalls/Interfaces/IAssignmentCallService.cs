using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface IAssignmentCallService
{
  Task<Response<AssignmentComplex[]>> GetByProjectId(int projectId);
  Task<Response<AssignmentRequest>> AddAssignment(AssignmentRequest assignment);
  Task<Response<AssignmentComplex>> MoveAssignmentToStage(int assignmentId, int stageId);
}
