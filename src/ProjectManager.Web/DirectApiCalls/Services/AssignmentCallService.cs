using Microsoft.CodeAnalysis;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class AssignmentCallService : ServiceBase, IAssignmentCallService
{
  public AssignmentCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<AssignmentComplex[]>> GetByProjectId(int projectId)
  {
    return await HttpClient.GetResponse<AssignmentComplex[]>($"/api/assignment?$filter=projectId eq {projectId}");
  }

  public async Task<Response<AssignmentComplex[]>> GetByUserId(int userId)
  {
    return await HttpClient.GetResponse<AssignmentComplex[]>($"/api/assignment?$filter=users/any(u: u/id eq {userId})");
  }

  public async Task<Response<AssignmentRequest>> AddAssignment(AssignmentRequest assignment)
  {
    return await HttpClient.Post<AssignmentRequest, AssignmentRequest>("/api/assignment", assignment);
  }

  public async Task<Response<AssignmentComplex>> MoveAssignmentToStage(int assignmentId, int stageId)
  {
    return await HttpClient.Patch<AssignmentComplex>($"/api/assignment/{assignmentId}/{stageId}");
  }

  public async Task<Response<AssignmentComplex>> SignUpUserToAssignment(int assignmentId, int userId)
  {
    return await HttpClient.Patch<AssignmentComplex>($"/api/assignment/signup/{assignmentId}/{userId}");
  }
}
