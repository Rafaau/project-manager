using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class UserById : Specification<User>, ISingleResultSpecification
{
  public UserById(int userId)
  {
    Query
      .Where(user => user.Id == userId);
  }
}
