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
using ProjectManager.Core.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.Modals;

public partial class RemoveConfirm
{
  [Inject] private IMessageCallService _messageService { get; set; }
  [Inject] private IJSRuntime _js { get; set; }
  [Parameter] public MessageComplex Message { get; set; }
  [Parameter] public EventCallback RefreshMessages { get; set; }

  private async Task Remove()
  {
    var response = await _messageService.RemoveMessage(Message.Id);
    if (response.IsSuccess)
    {
      await _js.InvokeVoidAsync("CloseModal", "#removeConfirm");
      await RefreshMessages.InvokeAsync();
    }
  }
}
