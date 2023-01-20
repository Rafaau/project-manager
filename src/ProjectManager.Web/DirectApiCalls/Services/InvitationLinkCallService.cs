using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.DirectApiCalls.Services;

public class InvitationLinkCallService : ServiceBase, IInvitationLinkCallService
{
  public InvitationLinkCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
  {
  }

  public async Task<Response<InvitationLinkComplex>> GenerateInvitation(InvitationLinkRequest request)
  {
    return await HttpClient.Post<InvitationLinkRequest, InvitationLinkComplex>("/api/invitationlink", request);
  }

  public async Task<Response<InvitationLinkComplex>> GetInvitationLink(string invitationLinkUrl)
  {
    return await HttpClient.GetResponse<InvitationLinkComplex>($"/api/invitationlink/{invitationLinkUrl}");
  }

  public async Task<Response<InvitationLinkComplex>> SetInvitationLinkAsUsed(int invitationLinkId)
  {
    return await HttpClient.Patch<InvitationLinkComplex>($"/api/invitationlink/{invitationLinkId}");
  }
}
