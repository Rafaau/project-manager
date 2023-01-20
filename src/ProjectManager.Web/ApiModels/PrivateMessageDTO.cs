namespace ProjectManager.Web.ApiModels;

public class PrivateMessageRequest
{
  public int SenderId { get; set; }
  public int ReceiverId { get; set; }
  public string Content { get; set; }
}

public class PrivateMessageComplex
{
  public int Id { get; set; }
  public UserSimplified Sender { get; set; }
  public UserSimplified Receiver { get; set; }
  public string Content { get; set; }
  public bool IsSeen { get; set; }
  public DateTime PostDate { get; set; }
}

public class PrivateMessageSimplified
{
  public int Id { get; set; }
  public string Content { get; set; }
  public DateTime PostDate { get; set; }
}
