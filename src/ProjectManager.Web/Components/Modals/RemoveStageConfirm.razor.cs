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
