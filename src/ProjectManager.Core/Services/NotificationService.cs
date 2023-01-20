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
public class NotificationService : INotificationService
{
  private readonly IRepository<Notification> _notificationRepository;
  private readonly IRepository<User> _userRepository;
  private readonly ILoggerAdapter<NotificationService> _logger;

  public NotificationService(
    IRepository<Notification> notificationRepository,
    IRepository<User> userRepository,
    ILoggerAdapter<NotificationService> logger)
  {
    _notificationRepository = notificationRepository;
    _userRepository = userRepository;
    _logger = logger;
  }

  public async Task<IQueryable<Notification>> RetrieveAllNotifications()
  {
    _logger.LogInformation("Retrieving all notifications");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var notifications = await _notificationRepository.ListAsync();

      return notifications.AsQueryable();
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving notifications");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Notifications retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Notification> CreateNotification(Notification request)
  {
    _logger.LogInformation("Creating notification");
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var userSpec = new UserById(request.UserId);
      var user = await _userRepository.FirstOrDefaultAsync(userSpec);
      request.User = user;

      var createdNotification = await _notificationRepository.AddAsync(request);
      return createdNotification;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while creating notification");
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Notification created in {0}ms", stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<Notification> SetNotificationAsSeen(int notificationId)
  {
    _logger.LogInformation("Setting notification (id: {0}) as seen", notificationId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var notificationSpec = new NotificationById(notificationId);
      var notificationToPatch = await _notificationRepository.FirstOrDefaultAsync(notificationSpec);
      notificationToPatch.IsSeen = true;

      await _notificationRepository.UpdateAsync(notificationToPatch);
      return notificationToPatch;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while setting notification (id {0}) as seen", notificationId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Notification (id: {0}) set as seen in {1}ms", notificationId, stopWatch.ElapsedMilliseconds);
    }
  }

}
