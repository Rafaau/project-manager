using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using ProjectManager.Web;
using ProjectManager.Web.Shared;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using Microsoft.AspNetCore.WebUtilities;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;

namespace ProjectManager.Web.Pages.ProjectPages;

public partial class ProjectChat
{
  [Inject] private IMessageCallService _messageService { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  [Inject] private IScrollHandler scroll { get; set; }
  [CascadingParameter] User User { get; set; }

  private ProjectComplex Project { get; set; }
  private class Model
  {
    public string Content { get; set; }
  }

  private Model model = new Model();
  private MessageComplex[] Messages { get; set; }
  private MessageComplex MessageToPass { get; set; }
  private int Id { get; set; }
  private int CurrentChannel { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
      Id = int.Parse(id);

    await RefreshMessages();

    var projectResponse = await _projectService.GetById(Id);
    if (projectResponse.IsSuccess)
    {
      Project = projectResponse.Data;
      if (Project.ChatChannels is not null)
        CurrentChannel = Project.ChatChannels.First().Id;
    }
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (!firstRender)
      await scroll.ScrollToElementByIdAsync("anchor");
  }

  private async Task RefreshMessages()
  {
    var messageResponse = await _messageService.GetByProjectId(Id);
    if (messageResponse.IsSuccess)
      Messages = messageResponse.Data;
  }
  private async Task Submit()
  {
    var message = new MessageRequest()
    {
      Content = model.Content,
      PostDate = DateTime.UtcNow,
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
  }

  private void PassMessageData(MessageComplex message)
  {
    MessageToPass = message;
  }

  private void SetCurrentChannel(int channelId)
  {
    CurrentChannel = channelId;
  }
}
