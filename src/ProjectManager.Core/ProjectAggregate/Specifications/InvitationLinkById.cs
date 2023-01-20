using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
