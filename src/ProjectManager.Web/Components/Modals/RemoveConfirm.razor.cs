using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
