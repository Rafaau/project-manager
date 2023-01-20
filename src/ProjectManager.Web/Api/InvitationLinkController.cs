using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class InvitationLinkController : BaseApiController
{
  private readonly IInvitationLinkService _invitationLinkService;
  private readonly IMapper _mapper;

  public InvitationLinkController(IInvitationLinkService invitationLinkService, IMapper mapper)
  {
    _invitationLinkService = invitationLinkService;
    _mapper = mapper;
  }

  [HttpPost] 
  public async Task<IActionResult> Post([FromBody] InvitationLinkRequest request)
  {
    try
    {
      var mapped = _mapper.Map<InvitationLink>(request);
      var generatedInvitationLink = await _invitationLinkService.GenerateInvitationLink(mapped);

      return CreatedAtAction(null, _mapper.Map<InvitationLinkComplex>(generatedInvitationLink).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpGet("{url}")]
  public async Task<IActionResult> Get(string url)
  {
    try
    {
      var retrievedInvitationLink = await _invitationLinkService.GetInvitationLink(url);

      return Ok(_mapper.Map<InvitationLinkComplex>(retrievedInvitationLink).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> Patch(int id)
  {
    try
    {
      var patchedInvitationLink = await _invitationLinkService.SetInvitationLinkAsUsed(id);

      return Ok(_mapper.Map<InvitationLinkComplex>(patchedInvitationLink).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }
}
