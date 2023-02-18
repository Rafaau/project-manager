using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.ProjectComponents;

public partial class ChatChannelForm
{
  [Inject] private IChatChannelCallService _channelService { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  [Parameter] public IWebHostEnvironment Env { get; set; }
  [Parameter] public ProjectComplex Project { get; set; }
  [Parameter] public EventCallback OnChannelCreate { get; set; }
  private class Model
  {
    public string Name { get; set; }
  }
  private Model model = new Model();
  private bool isPrivate { get; set; } = false;
  private List<UserSimplified> permissedUsers { get; set; } = new List<UserSimplified>();
  private int UserToRemove { get; set; } = 0;

  private async Task Submit()
  {
    permissedUsers.Add(Project.Manager);
    var request = new ChatChannelRequest()
    {
      Name = model.Name,
      ProjectId = Project.Id,
      PermissedUsers = isPrivate ? permissedUsers.ToArray() : Array.Empty<UserSimplified>()
    };

    await _channelService.CreateChatChannel(request);

    await js.InvokeVoidAsync("CloseDropdown", "#dropdownCreateChatChannel");
    await OnChannelCreate.InvokeAsync();
    permissedUsers.Clear();
    model = new Model();
  }

  private void UpdatePermissedUsers(UserSimplified user)
  {
    permissedUsers.Add(user);
  }
}
