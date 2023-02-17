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
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.Components.Modals;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class Conversations
{
  [Inject] IPrivateMessageCallService _messageService { get; set; }
  [Inject] IJSRuntime js { get; set; }
  [Parameter] public EventCallback onHideConversations { get; set; }
  [Parameter] public EventCallback onSetConversationAsSeen { get; set; }
  [CascadingParameter] User User { get; set; }
  private PrivateMessageComplex[] conversations { get; set; }
  private int receiverId { get; set; }
  Conversation conversation;

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (User != null && conversations == null)
    {
      await GetUserConversations();
      StateHasChanged();
      js.InvokeVoidAsync("AnimateConversations");
    }
  }

  private async Task GetUserConversations()
  {
    var response = await _messageService.GetUserConversations(User.Id);
    if (response.IsSuccess)
    {
      conversations = response.Data;
    }
  }

  private async Task HideConversations()
  {
    await onHideConversations.InvokeAsync();
  }

  private async Task PassId(int passedId)
  {
    receiverId = passedId;
    await conversation.GetMessages(passedId);
    var response = await _messageService.GetByUsers(receiverId, User.Id);
    if (response.IsSuccess)
    {
      foreach (var message in response.Data
        .Where(x => x.IsSeen == false 
        && (x.Receiver.Id == receiverId && x.Sender.Id == User.Id) 
        || (x.Sender.Id == receiverId && x.Receiver.Id == User.Id)))
        {
          await _messageService.SetMessageAsSeen(message.Id);
          await onSetConversationAsSeen.InvokeAsync();
        }
    }
  }
}
