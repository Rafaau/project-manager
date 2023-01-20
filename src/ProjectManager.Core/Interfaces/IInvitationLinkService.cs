using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IInvitationLinkService
{
  Task<InvitationLink> GenerateInvitationLink(InvitationLink request);
  Task<InvitationLink> GetInvitationLink(string invitationLinkUrl);
  Task<InvitationLink> SetInvitationLinkAsUsed(int invitationLinkId);
}
