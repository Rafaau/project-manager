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
using ProjectManager.Core.Interfaces;
using ProjectManager.Web.Components.DashboardComponents;
using System.Web.Mvc.Html;

namespace ProjectManager.Web.Pages;

public partial class Index
{
  [Inject] private IUserCallService _userService { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  UserTab userTab;
  private int UserCount { get; set; } = 0;
  private string message { get; set; } = "";
  private bool showUserSettings { get; set; } = false;

  protected override async Task OnInitializedAsync()
  {
    var response = await _userService.GetAllUsers();
    if (response != null)
      UserCount = response.Data.Count();

    var authState = await authenticationState;
    message = $"No siema, {authState.User.Identity.Name}";
  }

  private void ShowUserSettings()
  {
    showUserSettings =  !showUserSettings ? true : false;
  }

  private async Task SaveUserProfile()
  {
    await userTab.ShowUserSettings();
  }
}
