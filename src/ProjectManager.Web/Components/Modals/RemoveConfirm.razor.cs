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
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.Modals;

public partial class RemoveConfirm
{
  [Inject] private IChatMessageCallService _messageService { get; set; }
  [Inject] private IPrivateMessageCallService _pmService { get; set; }
  [Inject] private IJSRuntime _js { get; set; }
  [Parameter] public ChatMessageComplex Message { get; set; }
  [Parameter] public PrivateMessageComplex PrivateMessage { get; set; }
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

  private async Task RemovePM()
  {
    var response = await _pmService.DeleteMessage(PrivateMessage.Id);
    if (response.IsSuccess)
    {
      await _js.InvokeVoidAsync("CloseModal", "#removeConfirm");
      await RefreshMessages.InvokeAsync();
    }
  }
}
