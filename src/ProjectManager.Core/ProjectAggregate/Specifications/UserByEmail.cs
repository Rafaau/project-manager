using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class UserByEmail : Specification<User>
{
  public UserByEmail(string userEmail)
  {
    Query
      .Where(user => user.Email == userEmail);
  }
}
