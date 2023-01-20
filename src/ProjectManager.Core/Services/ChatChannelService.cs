using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class ChatChannelService : IChatChannelService
{
  private readonly IRepository<ChatChannel> _channelRepository;
  private readonly IRepository<User> _userRepository;
  private readonly ILoggerAdapter<ChatChannelService> _logger;

  public ChatChannelService(IRepository<ChatChannel> channelRepository, IRepository<User> userRepository, ILoggerAdapter<ChatChannelService> logger)
  {
    _channelRepository = channelRepository;
    _userRepository = userRepository;
    _logger = logger;
  }

  public async Task<ChatChannel> CreateChatChannel(ChatChannel request)
  {
    _logger.LogInformation("Creating chat channel");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var permissedUsers = new List<User>();
      foreach (var user in request.PermissedUsers)
      {
        var userSpec = new UserById(user.Id);
        var retrievedUser = await _userRepository.FirstOrDefaultAsync(userSpec);
        permissedUsers.Add(retrievedUser);
      }

      request.PermissedUsers = permissedUsers;
      var createdChannel = await _channelRepository.AddAsync(request);
      return createdChannel;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating channel");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Channel created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<ChatChannel> UpdateChatChannel(ChatChannel request)
  {
    _logger.LogInformation("Updating channel (id: {0})", request.Id);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var channelSpec = new ChatChannelById(request.Id);
      var channelToUpdate = await _channelRepository.FirstOrDefaultAsync(channelSpec);

      channelToUpdate.PermissedUsers.Clear();
      foreach (var user in request.PermissedUsers)
      {
        var userSpec = new UserById(user.Id);
        channelToUpdate.PermissedUsers.Add(await _userRepository.FirstOrDefaultAsync(userSpec));
      }

      await _channelRepository.UpdateAsync(channelToUpdate);
      return channelToUpdate;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while updating channel (id: {0})", request.Id);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Channel (id: {0}) updated in {1}ms", request.Id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<ChatChannel> DeleteChatChannel(int channelId)
  {
    _logger.LogInformation("Deleting chat channel (id: {0})", channelId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var channelSpec = new ChatChannelById(channelId);
      var channelToDelete = await _channelRepository.FirstOrDefaultAsync(channelSpec);

      await _channelRepository.DeleteAsync(channelToDelete);
      return channelToDelete;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while deleting chat channel (id: {0})", channelId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Chat channel (id: {0}) deleted in {1}ms", channelId, stopWatch.ElapsedMilliseconds);
    }
  }
}
