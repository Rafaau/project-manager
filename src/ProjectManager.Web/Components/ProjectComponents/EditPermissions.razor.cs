using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.ProjectComponents;

public partial class EditPermissions
{
  [Inject] private IChatChannelCallService _channelService { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  [Parameter] public ChatChannelComplex Channel { get; set; }
  [Parameter] public IWebHostEnvironment Env { get; set; }
  [Parameter] public ProjectComplex Project { get; set; }
  private List<UserSimplified> permissedUsers { get; set; } = new List<UserSimplified>();
  private int hoveredUser { get; set; } = 0;

  protected override async Task OnInitializedAsync()
  {
    permissedUsers = Channel.PermissedUsers.Length == 0 ? Project.Users.ToList() : Channel.PermissedUsers.ToList();
  }

  public void Refresh()
  {
    StateHasChanged();
  }

  private void UpdatePermissedUsers(UserSimplified user)
  {
    permissedUsers.Add(user);
  }

  private async Task UpdatePermissions()
  {
    var channelToUpdate = new ChatChannelSimplified()
    {
      Id = Channel.Id,
      PermissedUsers = permissedUsers.ToArray()
    };

    var response = await _channelService.UpdateChatChannel(channelToUpdate);

    if (response.IsSuccess)
      await js.InvokeVoidAsync("CloseDropdown", $"#{Channel.Id}-Dropdown");
  }
}
