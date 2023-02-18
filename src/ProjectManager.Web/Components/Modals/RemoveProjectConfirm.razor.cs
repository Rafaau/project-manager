using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.Modals;
public partial class RemoveProjectConfirm
{
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private IJSRuntime _js { get; set; }
  [Parameter] public ProjectComplex Project { get; set; }
  [Parameter] public EventCallback OnProjectDelete { get; set; }
  private string confirmInput { get; set; }
  private bool incorrectConfirm { get; set; } = false;

  protected override void OnAfterRender(bool firstRender)
  {
    if (incorrectConfirm)
      incorrectConfirm = false;
  }

  private async Task Delete()
  {
    if (confirmInput == Project.Name)
    {
      var response = await _projectService.DeleteProject(Project.Id);
      if (response.IsSuccess)
      {
        await OnProjectDelete.InvokeAsync();
        await _js.InvokeVoidAsync("CloseModal", "#removeProjectConfirm");
        confirmInput = "";
      }
    }
    else
      incorrectConfirm = true;
  }
}
