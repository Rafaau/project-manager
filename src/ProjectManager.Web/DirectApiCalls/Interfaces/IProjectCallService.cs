using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;
public interface IProjectCallService
{
  Task<Response<ProjectComplex[]>> GetUserProjects(int userId);
  Task<Response<ProjectComplex[]>> GetManagerProjects(int managerId);
  Task<Response<ProjectComplex>> GetById(int id);
  Task<Response<ProjectComplex>> AddProject(ProjectRequest project);
  Task<Response<ProjectComplex>> UpdateProject(ProjectComplex project);
  Task<Response<ProjectComplex>> DeleteProject(int projectId);
}
