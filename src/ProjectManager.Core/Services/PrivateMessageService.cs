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
public class PrivateMessageService : IPrivateMessageService
{
  private readonly IRepository<PrivateMessage> _messageRepository;
  private readonly IRepository<User> _userRepository;
  private readonly ILoggerAdapter<PrivateMessageService> _logger;

  public PrivateMessageService(
    IRepository<PrivateMessage> messageRepository,
    IRepository<User> userRepository,
    ILoggerAdapter<PrivateMessageService> logger)
  {
    _messageRepository = messageRepository;
    _userRepository = userRepository;
    _logger = logger;
  }

  public async Task<IQueryable<PrivateMessage>> RetrieveAllMessages()
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

  public async Task<PrivateMessage[]> GetUserConversations(int userId)
  {
    _logger.LogInformation("Retrieving user (id: {0}) conversations", userId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var conversations = (await _messageRepository.ListAsync())
        .Where(x => x.SenderId == userId || x.ReceiverId == userId)
        .OrderByDescending(x => x.PostDate)
        .DistinctBy(x => new { x.SenderId, x.ReceiverId })
        .ToArray();

      var idsToRemove = new List<int>();

      for (int i = 0; i < conversations.Length; i++)
      {
        if ((conversations[i].SenderId == userId && conversations.Any(x => x.PostDate > conversations[i].PostDate && x.SenderId == conversations[i].ReceiverId))
          ||(conversations[i].SenderId != userId && conversations.Any(x => x.PostDate > conversations[i].PostDate && x.SenderId == conversations[i].ReceiverId)))
        {
          //conversations = conversations.Where(x => x.Id != conversations[i].Id).ToArray();
          idsToRemove.Add(conversations[i].Id);
        }
      }

      conversations = conversations.Where(x => !idsToRemove.Contains(x.Id)).ToArray();

      return conversations;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving user (id: {0}) conversations", userId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("User (id: {0}) conversations retrieved in {1}ms", userId, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<PrivateMessage> PostMessage(PrivateMessage request)
  {
    _logger.LogInformation("Posting message");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var senderSpec = new UserById(request.SenderId);
      var sender = await _userRepository.FirstOrDefaultAsync(senderSpec);

      var receiverSpec = new UserById(request.ReceiverId);
      var receiver = await _userRepository.FirstOrDefaultAsync(receiverSpec);

      request.Sender = sender;
      request.Receiver = receiver;

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

  public async Task<PrivateMessage> SetMessageAsSeen(int messageId)
  {
    _logger.LogInformation("Setting message (id: {0}) as seen", messageId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var messageSpec = new PrivateMessageById(messageId);
      var messageToPatch = await _messageRepository.FirstOrDefaultAsync(messageSpec);
      messageToPatch.IsSeen = true;

      await _messageRepository.UpdateAsync(messageToPatch);
      return messageToPatch;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while setting message (id: {0}) as seen", messageId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Message (id: {0}) set as seen in {1}ms", messageId, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<PrivateMessage> EditMessage(PrivateMessage request)
  {
    _logger.LogInformation("Editing message (id: {0})", request.Id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var messageSpec = new PrivateMessageById(request.Id);
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

  public async Task<PrivateMessage> DeleteMessage(int id)
  {
    _logger.LogInformation("Deleting message (id: {0})", id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var messageSpec = new PrivateMessageById(id);
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
