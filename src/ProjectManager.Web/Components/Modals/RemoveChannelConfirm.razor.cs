using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
