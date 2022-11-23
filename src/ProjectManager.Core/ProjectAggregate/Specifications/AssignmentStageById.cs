using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace ProjectManager.Core.ProjectAggregate.Specifications;
public class AssignmentStageById : Specification<AssignmentStage>
{
  public AssignmentStageById(int id)
  {
    Query
      .Where(stage => stage.Id == id);
  }
}
