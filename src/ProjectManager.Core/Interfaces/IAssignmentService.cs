using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Core.Interfaces;
public interface IAssignmentService
{
  Task<IQueryable<Assignment>> RetrieveAllAssignments();
  Task<Assignment> CreateAssignment(Assignment request);
  Task<Assignment> UpdateAssignment(Assignment request);
  Task<Assignment> MoveAssignmentToStage(int assignmentId, int stageId);
  Task<Assignment> SignUpUserToAssignment(int assignmentId, int userId);
  Task<Assignment> DeleteAssignment(int id);
}
