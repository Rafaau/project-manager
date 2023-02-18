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
