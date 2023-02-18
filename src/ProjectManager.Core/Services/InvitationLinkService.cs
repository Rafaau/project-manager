using System.Diagnostics;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.Logging;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Specifications;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.Services;
public class InvitationLinkService : IInvitationLinkService
{
  private readonly IRepository<InvitationLink> _invitationLinkRepository;
  private readonly IRepository<Project2> _projectRepository;
  private readonly ILoggerAdapter<InvitationLinkService> _logger;

  public InvitationLinkService(
    IRepository<InvitationLink> invitationLinkRepository, 
    IRepository<Project2> projectRepository,
    ILoggerAdapter<InvitationLinkService> logger)
  {
    _invitationLinkRepository = invitationLinkRepository;
    _projectRepository = projectRepository;
    _logger = logger;
  }

  public async Task<InvitationLink> GenerateInvitationLink(InvitationLink request)
  {
    var stopWatch = Stopwatch.StartNew();
    _logger.LogInformation("Generating invitation link for project: {0}", request.ProjectId); 

    try
    {
      var projectSpec = new ProjectById(request.ProjectId);
      var project = await _projectRepository.FirstOrDefaultAsync(projectSpec);

      request.Project = project;
      request.Url = $"{Guid.NewGuid()}";
      var generatedInvitationLink = await _invitationLinkRepository.AddAsync(request);
      return generatedInvitationLink;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while generating invitation link for project: {0}", request.ProjectId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Invitation link for project {0} generated in {1}ms", request.Id, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<InvitationLink> GetInvitationLink(string invitationLinkUrl)
  {
    _logger.LogInformation("Retrieving invitation link (url: {0})", invitationLinkUrl);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var invitationLinkSpec = new InvitationLinkByUrl(invitationLinkUrl);
      var invitationLink = await _invitationLinkRepository.FirstOrDefaultAsync(invitationLinkSpec);

      return invitationLink;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while retrieving invitation link (url: {0})", invitationLinkUrl);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Invitation link (url: {0}) retrieved in {1}ms", invitationLinkUrl, stopWatch.ElapsedMilliseconds);
    }
  }

  public async Task<InvitationLink> SetInvitationLinkAsUsed(int invitationLinkId)
  {
    _logger.LogInformation("Setting invitation link (id : {0}) as used", invitationLinkId);
    var stopWatch = Stopwatch.StartNew();

    try
    {
      var invitationLinkSpec = new InvitationLinkById(invitationLinkId);
      var invitationLink = await _invitationLinkRepository.FirstOrDefaultAsync(invitationLinkSpec);

      invitationLink.IsUsed = true;
      await _invitationLinkRepository.UpdateAsync(invitationLink);
      return invitationLink;
    }
    catch (Exception e)
    {
      _logger.LogError(e, "Something went wrong while setting invitation link {0} as used", invitationLinkId);
      throw;
    }
    finally
    {
      stopWatch.Stop();
      _logger.LogInformation("Invitation link {0} set as used in {1}ms", invitationLinkId, stopWatch.ElapsedMilliseconds);
    }
  }
}
