namespace ProjectManager.Web.ApiModels;

public class ChatChannelRequest
{
  public string Name { get; set; }
  public int ProjectId { get; set; }
}

public class ChatChannelComplex
{
  public int Id { get; set; }
  public string Name { get; set; }
  public int ProjectId { get; set; }
  public UserSimplified[]? PermissedUsers { get; set; }
  public MessageComplex[]? Messages { get; set; }

  public ChatChannelComplex(string name, int projectId)
  {
    Name = name;
    ProjectId = projectId;
  }
}


