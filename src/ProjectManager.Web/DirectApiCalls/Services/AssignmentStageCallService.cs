using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class AssignmentStageCallService : ServiceBase, IAssignmentStageCallService
{
  public AssignmentStageCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<AssignmentStageComplex>> EditAssignmentStageName(int stageId, string name)
  {
    return await HttpClient.Patch<AssignmentStageComplex>($"/api/AssignmentStage/{stageId}/{name}");
  }

  public async Task<Response<AssignmentStageComplex>> AddAssignmentStage(AssignmentStageRequest request)
  {
    return await HttpClient.Post<AssignmentStageRequest, AssignmentStageComplex>("/api/AssignmentStage", request);
  }

  public async Task<Response<AssignmentStageComplex>> DeleteAssignmentStage(int stageId)
  {
    return await HttpClient.Delete<AssignmentStageComplex>($"/api/AssignmentStage/{stageId}");
  }
}
