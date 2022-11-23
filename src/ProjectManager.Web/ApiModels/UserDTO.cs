using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ProjectManager.Core.ProjectAggregate.Enums;

namespace ProjectManager.Web.ApiModels;

[DataContract]
public class UserRequest
{
  [JsonIgnore]
  public int Id { get; set; }
  [DataMember]
  public string Firstname { get; set; }
  [DataMember]
  public string Lastname { get; set; }
  [DataMember]
  public string Email { get; set; }
  [DataMember]
  public string Password { get; set; }
  [DataMember]
  public UserRole Role { get; set; }
}

public class UserComplex
{
  public int Id { get; set; }
  public string Firstname { get; set; }
  public string Lastname { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }
  public UserRole Role { get; set; }
  public List<ProjectSimplified>? ManagedProjects { get; set; }
  public List<ProjectSimplified>? Projects { get; set; }
}

public class UserSimplified
{
  public int Id { get; set; }
  public string Firstname { get; set; }
  public string Lastname { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }
  public UserRole Role { get; set; }
}
