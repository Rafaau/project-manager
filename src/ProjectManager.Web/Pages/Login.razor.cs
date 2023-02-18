using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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
