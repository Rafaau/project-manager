using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Web.Components;

public partial class CascadeLayout
{
  [Inject] private IUserCallService _userService { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  public User User { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var authState = await authenticationState;

    var userResponse = await _userService.GetUserByEmail(authState.User.Claims.ElementAt(2).Value);
    if (userResponse != null)
      User = userResponse.Data;
  }

}
