using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IProjectService
{
  Task<IQueryable<Project2>> RetrieveAllProjects();
  Task<Project2> RetrieveProjectById(int id);
  Task<Project2> CreateProject(Project2 request);
  Task<Project2> UpdateProject(Project2 request);
  Task<Project2> DeleteProject(int id);
}
