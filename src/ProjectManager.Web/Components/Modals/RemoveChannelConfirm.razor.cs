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
using BlazorInputFile;
using System.IO;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.Modals;

public partial class RemoveChannelConfirm
{
  [Inject] private IChatChannelCallService _channelService { get; set; }
  [Parameter] public IJSRuntime js { get; set; }
  [Parameter] public ChatChannelComplex ChatChannel { get; set; }
  [Parameter] public EventCallback OnChannelDelete { get; set; }

  private async Task DeleteChannel()
  {
    var response = await _channelService.DeleteChatChannel(ChatChannel.Id);
    if (response.IsSuccess)
    {
      await js.InvokeVoidAsync("CloseModal", "#deleteChannelConfirm");
      await OnChannelDelete.InvokeAsync();
    }
  }
}
