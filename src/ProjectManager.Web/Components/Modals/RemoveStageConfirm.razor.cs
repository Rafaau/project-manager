using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.Modals;

public partial class RemoveStageConfirm
{
  [Inject] private IAssignmentStageCallService _stageService { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  [Parameter] public EventCallback OnStageDelete { get; set; }
  [Parameter] public int StageId { get; set; }
  private async Task DeleteStage()
  {
    var response = await _stageService.DeleteAssignmentStage(StageId);
    if (response != null)
    {
      await OnStageDelete.InvokeAsync();
      await js.InvokeVoidAsync("CloseModal", "#removeConfirm");
    }
  }
}
