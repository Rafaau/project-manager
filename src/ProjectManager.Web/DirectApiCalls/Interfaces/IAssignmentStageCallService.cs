using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface IAssignmentStageCallService
{
  Task<Response<AssignmentStageComplex>> EditAssignmentStageName(int stageId, string name);
  Task<Response<AssignmentStageComplex>> AddAssignmentStage(AssignmentStageRequest request);
  Task<Response<AssignmentStageComplex>> DeleteAssignmentStage(int stageId);
}
