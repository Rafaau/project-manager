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
using BlazorInputFile;
using System.IO;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.Modals;
public partial class GenerateInvitation
{
  [Inject] private IInvitationLinkCallService _invitationService { get; set; }
  [Inject] private NavigationManager _navManager { get; set; }
  [Parameter] public IJSRuntime Js { get; set; }
  [Parameter] public int ProjectId { get; set; }
  private string generatedLink { get; set; } = "";

  private async Task GenerateInvitationLink()
  {
    var response = await _invitationService.GenerateInvitation(new InvitationLinkRequest() { ProjectId = ProjectId });
    if (response.IsSuccess)
    {
      var baseUri = _navManager.ToAbsoluteUri(_navManager.BaseUri);
      generatedLink = $"{baseUri}invitation?url={response.Data.Url}&";
    }
  }

  private async Task CopyToClipboard()
  {
    await Js.InvokeVoidAsync("CopyToClipboard", "fieldToCopy");
  }
}
