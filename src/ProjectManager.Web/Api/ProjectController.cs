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

public class ProjectController : BaseApiController
{
  private readonly IProjectService _projectService;
  private readonly IMapper _mapper;

  public ProjectController(IMapper mapper, IProjectService projectService)
  {
    _mapper = mapper;
    _projectService = projectService;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<Project2> queryOptions)
  {
    try
    {
      var retrievedProjects =
        queryOptions.ApplyTo(await _projectService.RetrieveAllProjects());

      return Ok(_mapper.Map<ProjectSimplified[]>(retrievedProjects).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    try
    {
      var retrievedProject =
        await _projectService.RetrieveProjectById(id);

      if (retrievedProject is null)
        return NotFound();

      return Ok(_mapper.Map<ProjectComplex>(retrievedProject).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] ProjectRequest request)
  {
    try
    {
      var mapped = _mapper.Map<Project2>(request);

      var createdProject = await _projectService.CreateProject(mapped);

      return Ok(_mapper.Map<ProjectComplex>(createdProject).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPut]
  public async Task<IActionResult> Update([FromBody] ProjectComplex request)
  {
    try
    {
      var mapped = _mapper.Map<Project2>(request);

      var updatedProject = await _projectService.UpdateProject(mapped);

      return Ok(_mapper.Map<ProjectComplex>(updatedProject).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var deletedProject = await _projectService.DeleteProject(id);

      return Ok(_mapper.Map<ProjectComplex>(deletedProject).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }
}
