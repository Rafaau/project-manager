using System;
using System.Collections.Generic;
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
public class MessageService : IMessageService
{
  private readonly IRepository<Message> _messageRepository;
  private readonly IRepository<User> _userRepository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly ILoggerAdapter<MessageService> _logger;

  public MessageService(
    IRepository<Message> messageRepository, 
    IRepository<User> userRepository, 
    IRepository<Project2> projectRepository,
    ILoggerAdapter<MessageService> logger)
  {
    _messageRepository = messageRepository;
    _userRepository = userRepository;
    _projectRepository = projectRepository;
    _logger = logger;
  }
  public async Task<IQueryable<Message>> RetrieveAllMessages()
  {
    _logger.LogInformation("Retrieving all messages");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var messages = await _messageRepository.ListAsync();
      return messages.AsQueryable();
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving all messages");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("All messages retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Message> PostMessage(Message request)
  {
    _logger.LogInformation("Posting message");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var userSpec = new UserById(request.UserId);
      var user = await _userRepository.FirstOrDefaultAsync(userSpec);

      var projectSpec = new ProjectById(request.ProjectId);
      var project = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      request.User = user;
      request.Project = project;

      var createdMessage = await _messageRepository.AddAsync(request);

      return createdMessage;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while posting message");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Message posted in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Message> EditMessage(Message request)
  {
    _logger.LogInformation("Editing message (id: {0})", request.Id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var messageSpec = new MessageById(request.Id);
      var messageToEdit = await _messageRepository.FirstOrDefaultAsync(messageSpec);

      messageToEdit.Content = request.Content;

      await _messageRepository.UpdateAsync(messageToEdit);

      return messageToEdit;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while editing message (id: {0})", request.Id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Message (id: {0}) edited in {1}ms", request.Id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Message> DeleteMessage(int id)
  {
    _logger.LogInformation("Deleting message (id: {0})", id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var messageSpec = new MessageById(id);
      var messageToDelete = await _messageRepository.FirstOrDefaultAsync(messageSpec);

      await _messageRepository.DeleteAsync(messageToDelete);

      return messageToDelete;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while deleting message (id: {0})", id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Message (id: {0}) deleted in {1}ms", id, stopWatch.ElapsedMilliseconds);
    }
  }
}
