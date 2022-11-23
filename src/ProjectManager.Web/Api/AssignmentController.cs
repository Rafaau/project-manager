using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class AssignmentController : BaseApiController
{
  private readonly IAssignmentService _assignmentService;
  private readonly IMapper _mapper;

  public AssignmentController(IMapper mapper, IAssignmentService assignmentService)
  {
    _mapper = mapper;
    _assignmentService = assignmentService;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<Assignment> queryOptions)
  {
    try
    {
      var retrievedAssignments =
        queryOptions.ApplyTo(await _assignmentService.RetrieveAllAssignments());

      return Ok(_mapper.Map<AssignmentComplex[]>(retrievedAssignments).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] AssignmentRequest request)
  {
    try
    {
      var mapped = _mapper.Map<Assignment>(request);

      var createdAssignment = await _assignmentService.CreateAssignment(mapped);

      return Ok(_mapper.Map<AssignmentRequest>(createdAssignment).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPut]
  public async Task<IActionResult> Update([FromBody] AssignmentComplex request)
  {
    try
    {
      var mapped = _mapper.Map<Assignment>(request);

      var updatedAssignment = await _assignmentService.UpdateAssignment(mapped);

      return Ok(_mapper.Map<AssignmentComplex>(updatedAssignment).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("{assignmentId}/{stageId}")]
  public async Task<IActionResult> MoveToStage(int assignmentId, int stageId)
  {
    try
    {
      var patchedAssignment = await _assignmentService.MoveAssignmentToStage(assignmentId, stageId);

      return Ok(_mapper.Map<AssignmentComplex>(patchedAssignment).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var deletedAssignment = await _assignmentService.DeleteAssignment(id);

      return Ok(_mapper.Map<AssignmentComplex>(deletedAssignment).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }
}
