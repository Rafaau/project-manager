using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
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
