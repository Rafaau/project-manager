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
