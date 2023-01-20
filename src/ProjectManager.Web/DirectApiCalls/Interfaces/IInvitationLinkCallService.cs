using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.DirectApiCalls.Interfaces;

public interface IInvitationLinkCallService
{
  Task<Response<InvitationLinkComplex>> GenerateInvitation(InvitationLinkRequest request);
  Task<Response<InvitationLinkComplex>> GetInvitationLink(string invitationLinkUrl);
  Task<Response<InvitationLinkComplex>> SetInvitationLinkAsUsed(int invitationLinkId);
}
