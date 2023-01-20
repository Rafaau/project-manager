using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class PrivateMessageById : Specification<PrivateMessage>
{
  public PrivateMessageById(int messageId)
  {
    Query
      .Where(x => x.Id == messageId);
  }
}
