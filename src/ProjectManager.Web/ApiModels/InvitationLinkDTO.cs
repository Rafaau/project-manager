using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Web.ApiModels;

public class InvitationLinkRequest
{
  public int ProjectId { get; set; }
}

public class InvitationLinkComplex
{
  public int Id { get; set; }
  public string Url { get; set; }
  public int ProjectId { get; set; }
  public ProjectSimplified Project { get; set; }
  public bool IsUsed { get; set; }
  public DateTime ExpirationTime { get; set; }
}

