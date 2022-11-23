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

namespace ProjectManager.Web.Shared;

public partial class SideNav
{
  private bool collapseNavMenu = true;
  private string? SideNavCssClass => collapseNavMenu ? "collapse" : null;
  private void ToggleNavMenu()
  {
    collapseNavMenu = !collapseNavMenu;
  }
}
