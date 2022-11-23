using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ProjectManager.Web.ApiModels;

[DataContract]
public class AssignmentStageRequest
{
  [JsonIgnore]
  public int Id { get; set; }
  [DataMember]
  public int Index { get; set; }
  [JsonIgnore]
  public string Name { get; set; } = "New";
  [DataMember]
  public int ProjectId { get; set; }
  [JsonIgnore]
  public AssignmentSimplified[]? Assignments { get; set; } = Array.Empty<AssignmentSimplified>();
}

public class AssignmentStageComplex
{
  public int Id { get; set; }
  public int Index { get; set; }
  public string Name { get; set; }
  public AssignmentSimplified[]? Assignments { get; set; }
  public ProjectSimplified Project { get; set; }
}

public class AssignmentStageSimplified
{
  public int Id { get; set; }
  public int Index { get; set; }
  public string Name { get; set; }
  public int ProjectId { get; set; }

  public AssignmentStageSimplified(int index, string name, int projectId)
  {
    Index = index;
    Name = name;
    ProjectId = projectId;
  }
}
