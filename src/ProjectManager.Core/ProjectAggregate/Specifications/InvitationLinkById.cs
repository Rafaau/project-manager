using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class InvitationLinkById : Specification<InvitationLink>
{
  public InvitationLinkById(int id)
  {
    Query
      .Where(i => i.Id == id);
  }
}
