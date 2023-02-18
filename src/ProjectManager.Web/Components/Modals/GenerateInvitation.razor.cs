using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
