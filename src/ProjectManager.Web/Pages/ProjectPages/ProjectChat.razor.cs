using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using Microsoft.AspNetCore.WebUtilities;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;
using ProjectManager.Web.Components.ProjectComponents;
using AutoMapper;

namespace ProjectManager.Web.Pages.ProjectPages;

public partial class ProjectChat
{
  [Inject] private IChatMessageCallService _messageService { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private INotificationCallService _notificationService { get; set; }
  [Inject] private IWebHostEnvironment env { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  [Inject] private IScrollHandler scroll { get; set; }
  [Inject] private IMapper _mapper { get; set; }
  [CascadingParameter] User User { get; set; }

  private ProjectComplex Project { get; set; }
  private class Model
  {
    public string Content { get; set; } = string.Empty;
  }

  private Model model = new Model();
  private Model editModel = new Model();
  private ChatMessageComplex[] Messages { get; set; }
  private ChatMessageComplex MessageToPass { get; set; }
  private int Id { get; set; }
  private int CurrentChannel { get; set; }
  private bool showMentions { get; set; } = false;
  private List<string> membersFullnames { get; set; } = new List<string>();
  private List<string> membersFullnames2 { get; set; } = new List<string>();
  private UserSimplified[] usersToMention { get; set; }
  private string currentContent { get; set; }
  private string currentMention { get; set; }
  private int messageToEdit { get; set; } = 0;
  private ChatChannelComplex chosenChannel { get; set; } = new();
  private UserDetails details;
  private EditPermissions permissions;

  protected override async Task OnInitializedAsync()
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
      Id = int.Parse(id);

    await RefreshMessages();
    await RefreshProject();
  }

  private async Task RefreshProject()
  {
    var projectResponse = await _projectService.GetById(Id);
    if (projectResponse.IsSuccess)
    {
      Project = projectResponse.Data;
      if (Project.ChatChannels is not null)
        CurrentChannel = Project.ChatChannels.OrderBy(x => x.Id).First().Id;
      if (Project.Users is not null)
      {
        usersToMention = Project.Users;
        var list = usersToMention.ToList();
        list.Add(Project.Manager);
        usersToMention = list.ToArray();
        foreach (var member in Project.Users)
        {
          membersFullnames.Add($"@{member.Firstname} {member.Lastname}");
          membersFullnames2.Add($"@{member.Firstname} {member.Lastname}");
        }
          membersFullnames.Add($"@{Project.Manager.Firstname} {Project.Manager.Lastname}");
      }
    }
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (!firstRender && messageToEdit == 0) // to avoid scrolling after edit click
      await scroll.ScrollToElementByIdAsync("anchor");
  }

  private void OnInputChange(ChangeEventArgs e)
  {
    currentContent = e.Value.ToString();
    currentMention = "";

    if (currentContent.Contains("@"))
      currentMention = currentContent.Substring(currentContent.LastIndexOf("@"), currentContent.Length - currentContent.LastIndexOf("@"));

    if (currentMention.EndsWith("@") || (currentMention.Contains("@") && !membersFullnames.ToArray().Any(s => currentMention.Contains(s))))
    {
      showMentions = true;
    }
    else
      showMentions = false;
  }

  private void MentionUser(string fullname)
  {
    model.Content = model.Content.Substring(0, model.Content.LastIndexOf("@"));
    model.Content += fullname;
    showMentions = false;
  }

  private async Task RefreshMessages()
  {
    var messageResponse = await _messageService.GetByProjectId(Id);
    if (messageResponse.IsSuccess)
      Messages = messageResponse.Data;
  }
  private async Task Submit()
  {
    foreach (var userToMention in membersFullnames)
    {
      if (model.Content.Contains(userToMention))
      {
        var userId = Project.Users.FirstOrDefault(x => $"@{x.Firstname} {x.Lastname}" == userToMention).Id;
        await _notificationService.CreateNotification(new NotificationRequest() 
          { Content = $"You have been mentioned in #{Project.ChatChannels.First(x => x.Id == CurrentChannel).Name} chat in {Project.Name}", 
            UserId = userId });
      }
    }
    var message = new ChatMessageRequest()
    {
      Content = model.Content,
      UserId = User.Id,
      ProjectId = Project.Id,
      ChatChannelId = CurrentChannel,
    };

    var response = await _messageService.AddMessage(message);
    if (response.IsSuccess)
    {
      await RefreshMessages();
      model = new Model();
      await scroll.ScrollToElementByIdAsync("anchor");
    }

    showMentions = false;
  }

  private async Task Update()
  {
    var response = await _messageService.EditMessage(messageToEdit, editModel.Content);
    if (response.IsSuccess)
    {
      await RefreshMessages();
      await scroll.ScrollToElementByIdAsync("anchor");
      messageToEdit = 0;
    }
  }

  private void PassMessageData(ChatMessageComplex message)
  {
    MessageToPass = message;
  }

  private void SetCurrentChannel(int channelId)
  {
    CurrentChannel = channelId;
  }

}
