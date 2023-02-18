using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class ChatMessageController : BaseApiController
{
  private readonly IChatMessageService _messageService;
  private readonly IRepository<ChatMessage> _repository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly IRepository<User> _userRepository;
  private readonly IMapper _mapper;

  public ChatMessageController(IChatMessageService messageService, IMapper mapper)
  {
    _messageService = messageService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<ChatMessage> queryOptions)
  {
    try
    {
      var retrievedMessages =
        queryOptions.ApplyTo(await _messageService.RetrieveAllMessages());

      return Ok(_mapper.Map<ChatMessageComplex[]>(retrievedMessages).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] ChatMessageRequest request)
  {
    try
    {
      var mapped = _mapper.Map<ChatMessage>(request);

      var createdMessage = await _messageService.PostMessage(mapped);

      return CreatedAtAction(null, _mapper.Map<ChatMessageComplex>(createdMessage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("{messageId}/{content}")]
  public async Task<IActionResult> Patch(int messageId, string content)
  {
    try
    {
      var editedMessage = await _messageService.EditMessage(messageId, content);

      return Ok(_mapper.Map<ChatMessageComplex>(editedMessage).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(NullReferenceException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var deletedMessage = await _messageService.DeleteMessage(id);

      return Ok(_mapper.Map<ChatMessageComplex>(deletedMessage).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(ArgumentNullException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }
}
