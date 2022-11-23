using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class AssignmentService : IAssignmentService
{
  private readonly IRepository<Assignment> _assignmentRepository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly IRepository<User> _userRepository;
  private readonly ILoggerAdapter<AssignmentService> _logger;
  private readonly IRepository<AssignmentStage> _stageRepository;

  public AssignmentService(
    IRepository<Assignment> assignmentRepository, 
    IRepository<Project2> projectRepository, 
    IRepository<User> userRepository,
    IRepository<AssignmentStage> stageRepository,
    ILoggerAdapter<AssignmentService> logger)
  {
    _assignmentRepository = assignmentRepository;
    _projectRepository = projectRepository;
    _userRepository = userRepository;
    _stageRepository = stageRepository;
    _logger = logger;
  }

  public async Task<IQueryable<Assignment>> RetrieveAllAssignments()
  {
    _logger.LogInformation("Retrieving all assignments");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var assignments = await _assignmentRepository.ListAsync();
      return assignments.AsQueryable();
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving all assignments");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("All assignments retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Assignment> CreateAssignment(Assignment request)
  {
    _logger.LogInformation("Creating assignment");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var users = new List<User>();

      foreach (var user in request.Users)
      {
        var userSpec = new UserById(user.Id);
        var userToAdd = await _userRepository.FirstOrDefaultAsync(userSpec);
        users.Add(userToAdd);
      }

      var projectSpec = new ProjectById(request.ProjectId);
      var project = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      var stageSpec = new AssignmentStageById(request.AssignmentStageId);
      var stage = await _stageRepository.FirstOrDefaultAsync(stageSpec);

      request.Project = project;
      request.AssignmentStage = stage;
      request.Users = users;

      var createdAssignment = await _assignmentRepository.AddAsync(request);
      return createdAssignment;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating assignment");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Assignment> UpdateAssignment(Assignment request)
  {
    _logger.LogInformation("Updating assignment (id: {0})", request.Id);
    var stopWatch = Stopwatch.StartNew();

    try 
    { 
      var assignmentSpec = new AssignmentById(request.Id);
      var assignmentToUpdate = await _assignmentRepository.FirstOrDefaultAsync(assignmentSpec);

      var projectSpec = new ProjectById(request.Id);
      var project = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      assignmentToUpdate.Users.Clear();
      foreach (var user in request.Users)
      {
        var userSpec = new UserById(user.Id);
        var userToAdd = await _userRepository.FirstOrDefaultAsync(userSpec);
        assignmentToUpdate.Users.Add(userToAdd);
      }

      assignmentToUpdate.Project = project;
      assignmentToUpdate.Name = request.Name;
      assignmentToUpdate.Description = request.Description;
      assignmentToUpdate.Deadline = request.Deadline;
      assignmentToUpdate.Priority = request.Priority;

      await _assignmentRepository.UpdateAsync(assignmentToUpdate);
      return assignmentToUpdate;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while updating assignment (id: {0})", request.Id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment (id: {0}) updated in {1}ms", request.Id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Assignment> MoveAssignmentToStage(int assignmentId, int stageId)
  {
    _logger.LogInformation("Moving assignment (id: {0}) to stage (id: {1})", assignmentId, stageId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var assignmentSpec = new AssignmentById(assignmentId);
      var assignmentToPatch = await _assignmentRepository.FirstOrDefaultAsync(assignmentSpec);

      //var stageSpec = new AssignmentStageById(stageId);
      //var stage = await _stageRepository.FirstOrDefaultAsync(stageSpec);

      assignmentToPatch.AssignmentStageId = stageId;

      await _assignmentRepository.UpdateAsync(assignmentToPatch);
      return assignmentToPatch;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while moving assignment (id: {0}) to stage (id: {1})", assignmentId, stageId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment (id: {0}) moved to stage (id: {1}) in {2}ms", assignmentId, stageId, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Assignment> DeleteAssignment(int id)
  {
    _logger.LogInformation("Deleting assignment (id: {0})", id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var assignmentSpec = new AssignmentById(id);
      var assignmentToDelete = await _assignmentRepository.FirstOrDefaultAsync(assignmentSpec);

      await _assignmentRepository.DeleteAsync(assignmentToDelete);
      return assignmentToDelete;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while deleting assignment (id: {0})", id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment (id: {0}) deleted in {1}ms", id, stopWatch.ElapsedMilliseconds);
    }
  }
}
