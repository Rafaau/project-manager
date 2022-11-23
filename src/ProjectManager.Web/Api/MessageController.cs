using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class MessageController : BaseApiController
{
  private readonly IMessageService _messageService;
  private readonly IRepository<Message> _repository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly IRepository<User> _userRepository;
  private readonly IMapper _mapper;

  public MessageController(IMessageService messageService, IMapper mapper)
  {
    _messageService = messageService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<Message> queryOptions)
  {
    try
    {
      var retrievedMessages =
        queryOptions.ApplyTo(await _messageService.RetrieveAllMessages());

      return Ok(_mapper.Map<MessageComplex[]>(retrievedMessages).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] MessageRequest request)
  {
    try
    {
      var mapped = _mapper.Map<Message>(request);

      var createdMessage = await _messageService.PostMessage(mapped);

      return Ok(_mapper.Map<MessageRequest>(createdMessage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("content")]
  public async Task<IActionResult> Patch([FromBody] MessageComplex request)
  {
    try
    {
      var mapped = _mapper.Map<Message>(request);

      var editedMessage = await _messageService.EditMessage(mapped);

      return Ok(_mapper.Map<MessageComplex>(editedMessage).Success());
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
      var deletedMessage = await _messageService.DeleteMessage(id);

      return Ok(_mapper.Map<MessageComplex>(deletedMessage).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }
}
