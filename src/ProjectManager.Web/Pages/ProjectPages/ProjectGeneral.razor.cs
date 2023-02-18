using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Pages.ProjectPages;

public partial class ProjectGeneral
{
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  private int Id { get; set; }

  private ProjectComplex Project { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
      Id = int.Parse(id);
    var projectResponse = await _projectService.GetById(Id);
    if (projectResponse != null)
      Project = projectResponse.Data;
  }
}
