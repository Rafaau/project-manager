using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
