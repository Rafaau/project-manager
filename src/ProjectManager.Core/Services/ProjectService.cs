using System.Diagnostics;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class ProjectService : IProjectService
{
  private readonly IRepository<Project2> _projectRepository;
  private readonly IRepository<User> _userRepository;
  private readonly ILoggerAdapter<ProjectService> _logger;

  public ProjectService(
    IRepository<Project2> projectRepository,
    IRepository<User> userRepository,
    ILoggerAdapter<ProjectService> logger)
  {
    _projectRepository = projectRepository;
    _userRepository = userRepository;
    _logger = logger;
  }

  public async Task<IQueryable<Project2>> RetrieveAllProjects()
  {
    _logger.LogInformation("Retrieving all projects");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var projects = await _projectRepository.ListAsync();

      return projects.AsQueryable();
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving all projects");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Projects retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Project2> RetrieveProjectById(int id)
  {
    _logger.LogInformation("Retrieving project with id: {0}", id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var projectSpec = new ProjectById(id);

      var project = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      return project;
    }
    catch (Exception e)
    {
      _logger.LogError(e, $"Something went wrong while retrieving project (id: {id})");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Project (id: {0}) retrieved in {1}ms", id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Project2> CreateProject(Project2 request)
  {
    _logger.LogInformation("Creating project");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var userSpec = new UserById(request.ManagerId);
      var manager = await _userRepository.FirstOrDefaultAsync(userSpec);

      request.Manager = manager;

      var createdProject = await _projectRepository.AddAsync(request);

      return createdProject;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating project");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Project created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Project2> UpdateProject(Project2 request)
  {
    _logger.LogInformation("Updating project (id: {0})", request.Id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var projectSpec = new ProjectById(request.Id);
      var projectToUpdate = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      projectToUpdate.Users.Clear();

      foreach (var user in request.Users)
      {
        var userSpec = new UserById(user.Id);
        var userToAdd = await _userRepository.FirstOrDefaultAsync(userSpec);
        projectToUpdate.Users.Add(userToAdd);
      }

      projectToUpdate.Name = request.Name;

      await _projectRepository.UpdateAsync(projectToUpdate);
      return projectToUpdate;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while updating project (id :{0})", request.Id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Project (id: {0}) updated in {1}ms", request.Id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Project2> DeleteProject(int id)
  {
    _logger.LogInformation("Deleting project (id: {0})", id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var projectSpec = new ProjectById(id);
      var projectToDelete = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      await _projectRepository.DeleteAsync(projectToDelete);
      return projectToDelete;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while deleting project (id: {0})", id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Project (id: {0}) deleted in {1}ms", id, stopWatch.ElapsedMilliseconds);
    }
  }
}
