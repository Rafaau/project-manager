using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class ChatChannelById : Specification<ChatChannel>
{
  public ChatChannelById(int id)
  {
    Query
      .Where(x => x.Id == id);
  }
}
