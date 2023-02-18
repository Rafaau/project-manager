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
