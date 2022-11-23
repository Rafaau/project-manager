using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
