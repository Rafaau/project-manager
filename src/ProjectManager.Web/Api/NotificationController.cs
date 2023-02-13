using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Api;

public class NotificationController : BaseApiController
{
  private readonly INotificationService _notificationService;
  private readonly IMapper _mapper;

  public NotificationController(INotificationService notificationService, IMapper mapper)
  {
    _notificationService = notificationService;
    _mapper = mapper;
  }

  [HttpGet]
  public async Task<IActionResult> List(ODataQueryOptions<Notification> queryOptions)
  {
    try
    {
      var retrievedNotifications =
        queryOptions.ApplyTo(await _notificationService.RetrieveAllNotifications());

      return Ok(_mapper.Map<NotificationComplex[]>(retrievedNotifications).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromBody] NotificationRequest request)
  {
    try
    {
      var mapped = _mapper.Map<Notification>(request);

      var createdNotification = await _notificationService.CreateNotification(mapped);

      return CreatedAtAction(null, _mapper.Map<NotificationComplex>(createdNotification).Success());
    }
    catch (Exception e)
    {
      return this.ReturnErrorResult(e);
    }
  }

  [HttpPatch("{notificationId}")]
  public async Task<IActionResult> SetAsSeen(int notificationId)
  {
    try
    {
      var patchedNotification = await _notificationService.SetNotificationAsSeen(notificationId);

      return Ok(_mapper.Map<NotificationComplex>(patchedNotification).Success());
    }
    catch (Exception e)
    {
      if (e.GetType() == typeof(NullReferenceException))
        return NotFound();
      return this.ReturnErrorResult(e);
    }
  }
}
