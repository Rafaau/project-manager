using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ProjectManager.Web.ApiModels;

[DataContract]
public class ProjectRequest
{
  [JsonIgnore]
  public static int Id { get; set; }
  [DataMember]
  public string Name { get; set; }
  [DataMember]
  public int ManagerId { get; set; }
  [JsonIgnore]
  public UserSimplified[]? Users { get; set; } = Array.Empty<UserSimplified>();
  [JsonIgnore]
  public AssignmentStageSimplified[]? AssignmentStages { get; set; } = new[]
  {
    new AssignmentStageSimplified(1, "To Do", Id),
    new AssignmentStageSimplified(2, "In Progress", Id),
    new AssignmentStageSimplified(3, "Done", Id),
  };
  [JsonIgnore]
  public ChatChannelComplex[]? ChatChannels { get; set; } = new[]
  {
    new ChatChannelComplex("General", Id)
  };
}

public class ProjectComplex
{
  public int Id { get; set; }
  public string Name { get; set; }
  public int ManagerId { get; set; }
  public UserSimplified Manager { get; set; }
  public UserSimplified[]? Users { get; set; }
  public AssignmentSimplified[]? Assignments { get; set; }
  public AssignmentStageSimplified[]? AssignmentStages { get; set; }
  public ChatChannelComplex[]? ChatChannels { get; set; }
}

public class ProjectSimplified
{
  public int Id { get; set; }
  public string Name { get; set; }
  public int ManagerId { get; set; }
  public UserSimplified Manager { get; set; }
  public UserSimplified[]? Users { get; set; }
}
