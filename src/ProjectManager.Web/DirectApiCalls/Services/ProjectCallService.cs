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

  public async Task<Response<ProjectComplex>> GetById(int id)
  {
    return await HttpClient.GetResponse<ProjectComplex>($"/api/Project/{id}");
  }

  public async Task<Response<Project2>> AddProject(Project2 project)
  {
    return await HttpClient.Post<Project2, Project2>("/api/Project", project);
  }

  public async Task<Response<ProjectComplex>> UpdateProject(ProjectComplex project)
  {
    return await HttpClient.Put<ProjectComplex, ProjectComplex>("/api/Project", project);
  }
}
