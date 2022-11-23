using System.Runtime.Serialization;
using ProjectManager.Core.ProjectAggregate.Enums;

namespace ProjectManager.Web.ApiModels;

public class AssignmentRequest
{
  public string Name { get; set; }
  public string Description { get; set; }
  public Priority Priority { get; set; }
  public DateTime Deadline { get; set; }
  public int ProjectId { get; set; }
  public UserSimplified[]? Users { get; set; }
  public int AssignmentStageId { get; set; }
}

public class AssignmentComplex
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public Priority Priority { get; set; }
  public DateTime Deadline { get; set; }
  public ProjectSimplified Project { get; set; }
  public UserSimplified[] Users { get; set; }
  public AssignmentStageSimplified AssignmentStage { get; set; }
}

public class AssignmentSimplified
{
  public int Id { get; set; }
  public string Name { get; set; }
  public ProjectSimplified Project { get; set; }
  public AssignmentStageSimplified AssignmentStage { get; set; }
}
