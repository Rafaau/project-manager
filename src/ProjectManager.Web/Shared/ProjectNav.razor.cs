using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Shared;

public partial class ProjectNav
{
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  private int Id { get; set; }
  private ProjectComplex Project { get; set; }
  private string pm = "/pm";
  private bool collapseNavMenu = true;
  private string? SideNavCssClass => collapseNavMenu ? "collapse" : null;
  private void ToggleNavMenu()
  {
    collapseNavMenu = !collapseNavMenu;
  }

  protected override async Task OnInitializedAsync()
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
      Id = Int32.Parse(id);
    var projectResponse = await _projectService.GetById(Id);
    if (projectResponse.IsSuccess)
      Project = projectResponse.Data;
  }
}
