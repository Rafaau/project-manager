using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
