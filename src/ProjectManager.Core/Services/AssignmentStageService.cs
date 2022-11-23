using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class AssignmentStageService : IAssignmentStageService
{
  private readonly IRepository<AssignmentStage> _stageRepository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly ILoggerAdapter<AssignmentStageService> _logger;

  public AssignmentStageService(
    IRepository<AssignmentStage> stageRepository,
    IRepository<Project2> projectRepository,
    ILoggerAdapter<AssignmentStageService> logger)
  {
    _stageRepository = stageRepository;
    _projectRepository = projectRepository;
    _logger = logger;
  }

  public async Task<AssignmentStage> AddAssignmentStage(AssignmentStage request)
  {
    _logger.LogInformation("Creating new assignment stage");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var projectSpec = new ProjectById(request.ProjectId);
      var project = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      request.Project = project;

      var createdStage = await _stageRepository.AddAsync(request);
      return createdStage;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating assignment stage");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment stage created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<AssignmentStage> UpdateAssignmentStage(int stageId, string name)
  {
    _logger.LogInformation("Updating assignment stage (id: {0})", stageId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var stageSpec = new AssignmentStageById(stageId);
      var stageToUpdate = await _stageRepository.FirstOrDefaultAsync(stageSpec);

      stageToUpdate.Name = name;

      await _stageRepository.UpdateAsync(stageToUpdate);
      return stageToUpdate;
    } 
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while updating assignment stage (id: {0})", stageId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment stage (id: {0}) updated in {1}ms", stageId, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<AssignmentStage> DeleteAssignmentStage(int stageId)
  {
    _logger.LogInformation("Deleting assignmment stage (id: {0})", stageId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var stageSpec = new AssignmentStageById(stageId);
      var stageToDelete = await _stageRepository.FirstOrDefaultAsync(stageSpec);

      await _stageRepository.DeleteAsync(stageToDelete);
      return stageToDelete;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while deleting assignment stage (id: {0})", stageId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Assignment stage (id: {0}) deleted in {1}ms", stageId, stopWatch.ElapsedMilliseconds);
    }
  }
}
