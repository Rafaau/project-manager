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
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class ManagerProjectsView
{
  [CascadingParameter] User User { get; set; }
  [Inject] NavigationManager navManager { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  private bool IsButtonHovered { get; set; } = false;
  private ProjectComplex ProjectToPass { get; set; } = new ProjectComplex();
  private ProjectComplex[] Projects { get; set; }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (Projects == null)
    {
      await RefreshProjects();
    }
  }

  private async Task RefreshProjects()
  {
    if (User != null)
    {
      var response = await _projectService.GetManagerProjects(User.Id);
      if (response.IsSuccess)
      {
        Projects = response.Data.OrderBy(x => x.Id).ToArray();
        StateHasChanged();
      }
    }
  }

  private void Redirect(string path)
  {
    navManager.NavigateTo($"/project/general?id={path}&");
  }

  public async Task Rerender()
  {

  }
}
