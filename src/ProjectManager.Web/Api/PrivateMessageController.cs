using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class PrivateMessageController : BaseApiController
{
  private readonly IPrivateMessageService _messageService;
  private readonly IMapper _mapper;

  public PrivateMessageController(IPrivateMessageService messageService, IMapper mapper)
  {
    _messageService = messageService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<PrivateMessage> queryOptions)
  {
    try
    {
      var retrievedMessages =
        queryOptions.ApplyTo(await _messageService.RetrieveAllMessages());

      return Ok(_mapper.Map<PrivateMessageComplex[]>(retrievedMessages).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpGet("{userId}")]
  public async Task<IActionResult> GetUserConversations(int userId)
  {
    try
    {
      var retrievedMessages = await _messageService.GetUserConversations(userId);

      return Ok(_mapper.Map<PrivateMessageComplex[]>(retrievedMessages).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] PrivateMessageRequest request)
  {
    try
    {
      var mapped = _mapper.Map<PrivateMessage>(request);

      var createdMessage = await _messageService.PostMessage(mapped);

      return CreatedAtAction(null, _mapper.Map<PrivateMessageComplex>(createdMessage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("{messageId}")]
  public async Task<IActionResult> SetAsSeen(int messageId)
  {
    try
    {
      var patchedMessage = await _messageService.SetMessageAsSeen(messageId);

      return Ok(_mapper.Map<PrivateMessageComplex>(patchedMessage).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(NullReferenceException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPut]
  public async Task<IActionResult> Update(PrivateMessageSimplified request)
  {
    try
    {
      var mapped = _mapper.Map<PrivateMessage>(request);

      var updatedMessage = await _messageService.EditMessage(mapped);

      return Ok(_mapper.Map<PrivateMessageComplex>(updatedMessage).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(NullReferenceException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete("{messageId}")]
  public async Task<IActionResult> Delete(int messageId)
  {
    try
    {
      var deletedMessage = await _messageService.DeleteMessage(messageId);

      return Ok(_mapper.Map<PrivateMessageComplex>(deletedMessage).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(ArgumentNullException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }
}
