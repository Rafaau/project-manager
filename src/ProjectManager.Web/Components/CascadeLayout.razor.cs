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
