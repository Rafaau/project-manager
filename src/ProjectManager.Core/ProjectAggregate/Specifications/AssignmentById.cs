using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class AssignmentById: Specification<Assignment>
{
  public AssignmentById(int assignmentId)
  {
    Query
      .Where(assignment => assignment.Id == assignmentId);
  }
}
