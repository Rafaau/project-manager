using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Interfaces;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class ProjectsTab
{
  [CascadingParameter] User User { get; set; }
  [Inject] NavigationManager navManager { get; set; }
  [Inject] private IUserCallService _userService { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  private void Redirect(string path)
  {
    navManager.NavigateTo($"/project/general?id={path}&");
  }
  private async Task Rerender()
  {
    var authState = await authenticationState;

    var userResponse = await _userService.GetUserByEmail(authState.User.Claims.ElementAt(2).Value);
    if (userResponse != null)
      User = userResponse.Data;

    StateHasChanged();
  }
}
