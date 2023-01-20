using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class MessageById : Specification<ChatMessage>
{
  public MessageById(int messageId)
  {
    Query
      .Where(message => message.Id == messageId);
  }
}
