using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class AssignmentStageController : BaseApiController
{
  private readonly IAssignmentStageService _stageService;
  private readonly IMapper _mapper;

  public AssignmentStageController(IAssignmentStageService stageService, IMapper mapper)
  {
    _stageService = stageService;
    _mapper = mapper;
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] AssignmentStageRequest request)
  {
    try
    {
      var mapped = _mapper.Map<AssignmentStage>(request);

      var createdStage = await _stageService.AddAssignmentStage(mapped);

      return Ok(_mapper.Map<AssignmentStageComplex>(createdStage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("{stageId}/{name}")]
  public async Task<IActionResult> EditAssignmentStageName(int stageId, string name)
  {
    try
    {
      var patchedStage = await _stageService.UpdateAssignmentStage(stageId, name);
      return Ok(_mapper.Map<AssignmentStageComplex>(patchedStage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete("{stageId}")]
  public async Task<IActionResult> Delete(int stageId)
  {
    try
    {
      var deletedStage = await _stageService.DeleteAssignmentStage(stageId);
      return Ok(_mapper.Map<AssignmentStageComplex>(deletedStage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }
}
