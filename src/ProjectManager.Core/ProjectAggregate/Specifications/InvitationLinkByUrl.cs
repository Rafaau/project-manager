using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class InvitationLinkByUrl : Specification<InvitationLink>
{
  public InvitationLinkByUrl(string url)
  {
    Query
      .Where(i => i.Url == url);
  }
}
