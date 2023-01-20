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
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Majorsoft.Blazor.Components.Common.JsInterop.Scroll;
using ProjectManager.Core.ProjectAggregate.Specifications;

namespace ProjectManager.Web.Components.Modals;

public partial class Conversation
{
  [Inject] private IPrivateMessageCallService _messageService { get; set; }
  [Inject] private IScrollHandler scroll { get; set; }
  [Inject] private IWebHostEnvironment env { get; set; }
  [Parameter] public int UserId { get; set; }
  [Parameter] public int ReceiverId { get; set; }
  private PrivateMessageComplex[] messages { get; set; } = Array.Empty<PrivateMessageComplex>();
  private int messageId { get; set; } = 0;

  private class MessageClass
  {
    public string Content { get; set; }
  }

  private MessageClass message = new MessageClass();
  private MessageClass messageToUpdate = new MessageClass();
  private PrivateMessageComplex messageToDelete { get; set; }
  private PrivateMessageSimplified messageToEdit { get; set; } = new PrivateMessageSimplified();
  private bool EditMode { get; set; } = false;
  private bool isUserAvatarExist { get; set; } = false;
  private bool isReceiverAvatarExist { get; set; } = false;

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (!firstRender && messages.Length != 0)
    {
      await Task.Delay(100);
      await scroll.ScrollToElementByIdAsync("anchor");
    }
  }

  public async Task GetMessages(int rec)
  {
    var response = await _messageService.GetByUsers(UserId, rec);
    
    if (response.IsSuccess)
    {
      messages = response.Data.OrderBy(x => x.PostDate).ToArray();
      CheckIfAvatarsExist(rec);
      StateHasChanged();
    }
  }

  public async Task GetMessages()
  {
    var response = await _messageService.GetByUsers(UserId, ReceiverId);

    if (response.IsSuccess)
    {
      messages = response.Data.OrderBy(x => x.PostDate).ToArray();
      StateHasChanged();
    }
  }

  private void ResetMessages()
  {
    messages = Array.Empty<PrivateMessageComplex>();
  }

  private void CheckIfAvatarsExist(int recId)
  {
    var userPath = Path.Combine(env.ContentRootPath, "wwwroot/avatars", $"pm-avatar-{UserId}.jpg");
    isUserAvatarExist = System.IO.File.Exists(userPath);
    var receiverPath = Path.Combine(env.ContentRootPath, "wwwroot/avatars", $"pm-avatar-{recId}.jpg");
    isReceiverAvatarExist = System.IO.File.Exists(receiverPath);
  }

  private async Task Submit()
  {
    var messageToSend = new PrivateMessageRequest();
    messageToSend.Content = message.Content;
    messageToSend.SenderId = UserId;
    messageToSend.ReceiverId = ReceiverId;

    var response = await _messageService.SendMessage(messageToSend);
    if (response.IsSuccess)
    {
      await GetMessages(ReceiverId);
      message.Content = string.Empty;
    }
  }

  private async Task Update()
  {
    var response = await _messageService.UpdateMessage(messageToEdit);
    if (response.IsSuccess)
    {
      await GetMessages(ReceiverId);
      messageToEdit = new PrivateMessageSimplified();
      EditMode = false;
    }
  }

  private void PassMessage(PrivateMessageComplex message)
  {
    messageToDelete = message;
  }
}
