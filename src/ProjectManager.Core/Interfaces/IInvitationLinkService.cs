using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IInvitationLinkService
{
  Task<InvitationLink> GenerateInvitationLink(InvitationLink request);
  Task<InvitationLink> GetInvitationLink(string invitationLinkUrl);
  Task<InvitationLink> SetInvitationLinkAsUsed(int invitationLinkId);
}
