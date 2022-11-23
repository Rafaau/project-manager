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
