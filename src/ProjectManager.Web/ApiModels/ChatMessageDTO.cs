namespace ProjectManager.Web.ApiModels;


public class ChatMessageRequest
{
  public string Content { get; set; }
  public int UserId { get; set; }
  public int ProjectId { get; set; }
  public int ChatChannelId { get; set; }
}
public class ChatMessageComplex
{
  public int Id { get; set; }
  public string Content { get; set; }
  public DateTime PostDate { get; set; }
  public UserSimplified User { get; set; }
  public ProjectSimplified Project { get; set; }
  public int ChatChannelId { get; set; }
}
