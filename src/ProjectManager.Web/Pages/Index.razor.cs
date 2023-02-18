using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Core.Interfaces;
using ProjectManager.Web.Components.DashboardComponents;

namespace ProjectManager.Web.Pages;

public partial class Index
{
  [Inject] private IUserCallService _userService { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  UserTab userTab;
  private int UserCount { get; set; } = 0;
  private string message { get; set; } = "";
  private bool showUserSettings { get; set; } = false;
  private bool showNotifications { get; set; } = false;
  private bool showConversations { get; set; } = false;

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

  private void ShowNotifications()
  {
    showNotifications = !showNotifications ? true : false;
  }

  private void ShowConversations()
  {
    showConversations = !showConversations ? true : false;
  }

  private async Task SaveUserProfile()
  {
    await userTab.ShowUserSettings();
  }

  private async Task HideNotifications()
  {
    await userTab.ShowNotifications();
  }

  private async Task HideConversations()
  {
    await userTab.ShowConversations();
  }

  private async Task MarkNotificationsAsSeen()
  {
    await userTab.MarkNotificationsAsSeen();
  }

  private async Task RefreshConversations()
  {
    await userTab.GetConversations();
  }
}
