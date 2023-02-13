using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class ChatChannelController : BaseApiController
{
  private readonly IChatChannelService _channelService;
  private readonly IMapper _mapper;

  public ChatChannelController(IChatChannelService channelService, IMapper mapper)
  {
    _channelService = channelService;
    _mapper = mapper;
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] ChatChannelRequest request)
  {
    try
    {
      var mapped = _mapper.Map<ChatChannel>(request);
      var createdChatChannel = await _channelService.CreateChatChannel(mapped);

      return CreatedAtAction(null, _mapper.Map<ChatChannelComplex>(createdChatChannel).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPut]
  public async Task<IActionResult> Update([FromBody] ChatChannelSimplified request)
  {
    try
    {
      var mapped = _mapper.Map<ChatChannel>(request);
      var updatedChatChannel = await _channelService.UpdateChatChannel(mapped);

      return Ok(_mapper.Map<ChatChannelComplex>(updatedChatChannel).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(NullReferenceException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete("{channelId}")]
  public async Task<IActionResult> Delete(int channelId)
  {
    try
    {
      var deletedChatChannel = await _channelService.DeleteChatChannel(channelId);

      return Ok(_mapper.Map<ChatChannelComplex>(deletedChatChannel).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(ArgumentNullException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }
}
