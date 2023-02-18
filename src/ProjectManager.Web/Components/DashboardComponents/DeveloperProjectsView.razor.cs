using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class DeveloperProjectsView
{
  [CascadingParameter] User User { get; set; }
  [Inject] NavigationManager navManager { get; set; }
  [Inject] private IUserCallService _userService { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  private void Redirect(string path)
  {
    navManager.NavigateTo($"/project/general?id={path}&");
  }
}
