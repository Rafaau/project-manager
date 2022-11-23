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
using ProjectManager.Web.Authentication;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using static System.Net.WebRequestMethods;
using System.Security.Policy;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class UserTab
{
  [Inject] private HttpClient Http { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  [Inject] AuthenticationStateProvider authStateProvider { get; set; }
  [Inject] NavigationManager navManager { get; set; }
  [Inject] private IUserCallService _userService { get; set; }
  [Parameter] public EventCallback onShowUserSettings { get; set; }
  private string UserName { get; set; } = string.Empty;
  [CascadingParameter] private User User { get; set; }
  private bool isSettingsOn { get; set; } = false;
  private string imageURL { get; set; }

  private async Task Logout()
  {
    var auth = (AuthStateProvider)authStateProvider;
    await auth.UpdateAuthenticationState(null, false);
    navManager.NavigateTo("/pm", true);
  }

  private void RedirectToProfile()
  {
    navManager.NavigateTo("/profile", true);
  }
  protected override async Task OnInitializedAsync()
  {

  }

  public async Task ShowUserSettings()
  {
    await onShowUserSettings.InvokeAsync();
    isSettingsOn = !isSettingsOn ?  true :  false;
  }
}
