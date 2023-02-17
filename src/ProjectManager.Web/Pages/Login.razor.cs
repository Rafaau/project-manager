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
using ProjectManager.Web.Api;
using ProjectManager.Core.Interfaces;
using ProjectManager.Web.Authentication;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Web.Pages;

public partial class Login
{
  [Inject] private IUserCallService _userService { get; set; }
  [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  [Inject] private IJSRuntime js { get; set; }

  private bool Remember { get; set; } = false;
  private bool isWrongData = false;

  private class Model
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  private Model model = new Model();

  private async Task Authenticate()
  {
    var userResponse = await _userService.GetUserByEmail(model.Email);
    var userAccount = userResponse.Data;
    if (userAccount == null || !userAccount.Password.VerifyHashedPassword(model.Password))
    {
      isWrongData = true;
      return;
    }

    var auth = (AuthStateProvider)authStateProvider;
    await auth.UpdateAuthenticationState(new User
    {
      Firstname = userAccount.Firstname,
      Lastname = userAccount.Lastname,
      Email = userAccount.Email,
      Role = userAccount.Role,
    }, Remember);

    navManager.NavigateTo("/pm", true);
  }
}
