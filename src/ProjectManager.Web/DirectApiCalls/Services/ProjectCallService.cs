using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;
public class ProjectCallService : ServiceBase, IProjectCallService
{
  public ProjectCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<ProjectComplex[]>> GetUserProjects(int userId)
  {
    return await HttpClient.GetResponse<ProjectComplex[]>($"/api/Project?$filter=users/any(u: u/id eq {userId})");
  }

  public async Task<Response<ProjectComplex[]>> GetManagerProjects(int managerId)
  {
    return await HttpClient.GetResponse<ProjectComplex[]>($"/api/Project?$filter=managerid eq {managerId}");
  }

  public async Task<Response<ProjectComplex>> GetById(int id)
  {
    return await HttpClient.GetResponse<ProjectComplex>($"/api/Project/{id}");
  }

  public async Task<Response<ProjectComplex>> AddProject(ProjectRequest project)
  {
    return await HttpClient.Post<ProjectRequest, ProjectComplex>("/api/Project", project);
  }

  public async Task<Response<ProjectComplex>> UpdateProject(ProjectComplex project)
  {
    return await HttpClient.Put<ProjectComplex, ProjectComplex>("/api/Project", project);
  }

  public async Task<Response<ProjectComplex>> DeleteProject(int projectId)
  {
    return await HttpClient.Delete<ProjectComplex>($"/api/Project/{projectId}");
  }
}
