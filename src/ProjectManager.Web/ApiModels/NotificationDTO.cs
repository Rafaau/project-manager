namespace ProjectManager.Web.ApiModels;

public class NotificationRequest
{
  public string Content { get; set; }
  public int UserId { get; set; }
}

public class NotificationComplex
{
  public int Id { get; set; }
  public string Content { get; set; }
  public UserSimplified User { get; set; }
  public bool IsSeen { get; set; }
  public DateTime Date { get; set; }
}
