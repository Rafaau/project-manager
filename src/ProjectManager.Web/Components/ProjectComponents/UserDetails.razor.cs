using Microsoft.AspNetCore.Components;
using ProjectManager.Web.ApiModels;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.ProjectComponents;

public partial class UserDetails
{
  [Inject] private IWebHostEnvironment env { get; set; }
  [Inject] private IUserCallService userService { get; set; }
  [Inject] private IPrivateMessageCallService _messageService { get; set; }
  [Parameter] public UserSimplified ViewedUser { get; set; }
  [CascadingParameter] User User { get; set; }
  private User userComplex { get; set; }
  private class Message
  {
    public string Content { get; set; }
  }
  private Message privateMessage = new Message();

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (userComplex is null || userComplex.Id != ViewedUser.Id)
    {
      var response = await userService.GetUserByEmail(ViewedUser.Email);
      if (response.IsSuccess)
      {
        userComplex = response.Data;
        StateHasChanged();
      }
    }
  }

  public void Refresh()
  {
    StateHasChanged();
  }

  private async Task Submit()
  {
    var message = new PrivateMessageRequest()
    {
      SenderId = User.Id,
      ReceiverId = ViewedUser.Id,
      Content = privateMessage.Content
    };

    var response = await _messageService.SendMessage(message);
    if (response.IsSuccess)
      privateMessage = new Message();
  }
}
