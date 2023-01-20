using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using ProjectManager.Web;
using ProjectManager.Web.Shared;
using ProjectManager.Core.ProjectAggregate;
using MediatR;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class Notifications
{
  [Inject] INotificationCallService _notificationService { get; set; }
  [Inject] IJSRuntime js { get; set; }
  [CascadingParameter] User User { get; set; }
  [Parameter] public EventCallback onHideNotifications { get; set; }
  [Parameter] public EventCallback MarkNotificationsAsSeen { get; set; }
  private NotificationComplex[] notifications { get; set; }
  private int unseenNotifications { get; set; } = 0;

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (User != null && firstRender)
    {
      await GetNotifications();
      StateHasChanged();
      js.InvokeVoidAsync("AnimateNotifications");
      if (notifications != null)
      {
        unseenNotifications = notifications.Where(x => !x.IsSeen).Count();
        await MarkNotificationsAsSeen.InvokeAsync();
      }
    }
  }

  private async Task GetNotifications()
  {
    var response = await _notificationService.GetByUserId(User.Id);
    if (response.IsSuccess)
      notifications = response.Data;
  }

  private async Task HideNotifications()
  {
    await onHideNotifications.InvokeAsync();
  }
}
