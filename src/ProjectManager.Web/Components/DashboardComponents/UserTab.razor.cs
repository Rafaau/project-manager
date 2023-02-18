using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Web.Authentication;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.FileServices.Interfaces;
using BlazorInputFile;
using ProjectManager.Core.ProjectAggregate.Enums;
using AutoMapper;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class UserTab
{
  [Inject] private HttpClient Http { get; set; }
  [CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
  [Inject] AuthenticationStateProvider authStateProvider { get; set; }
  [Inject] NavigationManager navManager { get; set; }
  [Inject] private IUserCallService _userService { get; set; }
  [Inject] private INotificationCallService _notificationService { get; set; }
  [Inject] private IPrivateMessageCallService _messageService { get; set; }
  [Inject] private IImageService _imageService { get; set; }
  [Inject] private IWebHostEnvironment env { get; set; }
  [Inject] private IMapper mapper { get; set; }
  [Parameter] public EventCallback onShowUserSettings { get; set; }
  [Parameter] public EventCallback onShowNotifications { get; set; }
  [Parameter] public EventCallback onShowConversations { get; set; }
  private string UserName { get; set; } = string.Empty;
  [CascadingParameter] private User User { get; set; }
  private bool isSettingsOn { get; set; } = false;
  private bool isNotificationsOn { get; set; } = false;
  private bool isConversationsOn { get; set; } = false;
  private string imageURL { get; set; }
  private NotificationComplex[] notifications { get; set; }
  private PrivateMessageComplex[] messages { get; set; }
  private IFileListEntry file;
  private bool isAvatarExist { get; set; } = false;
  private class UserFields
  {
    public string Email { get; set; }
    public int Specialization { get; set; }
    public string Password { get; set; }
    public string PasswordToConfirm { get; set; }
  }
  private UserFields userModel { get; set; } = new();
  private bool justUploaded { get; set; } = false;

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (User != null && notifications == null)
    {
      await GetNotifications();
      await GetConversations();
      await CheckIfAvatarExist();
      Init();
      StateHasChanged();
    }
  }

  private async Task Logout()
  {
    var auth = (AuthStateProvider)authStateProvider;
    await auth.UpdateAuthenticationState(null, false);
    navManager.NavigateTo("/pm", true);
  }

  private async Task CheckIfAvatarExist()
  {
    var path = Path.Combine(env.ContentRootPath, "wwwroot/avatars", $"pm-avatar-{User.Id}.jpg");
    isAvatarExist = System.IO.File.Exists(path);
  }

  private void RedirectToProfile()
  {
    navManager.NavigateTo("/profile", true);
  }

  public async Task ShowUserSettings()
  {
    await onShowUserSettings.InvokeAsync();
    isSettingsOn = !isSettingsOn ?  true :  false;
    justUploaded = false;
  }

  public async Task ShowNotifications()
  {
    await onShowNotifications.InvokeAsync();
    isNotificationsOn = !isNotificationsOn ? true : false;
  }

  public async Task ShowConversations()
  {
    await onShowConversations.InvokeAsync();
    isConversationsOn = !isConversationsOn ? true : false;
  }

  private async Task GetNotifications()
  {
    var response = await _notificationService.GetByUserId(User.Id);
    if (response.IsSuccess)
      notifications = response.Data;
  }

  public async Task GetConversations()
  {
    var response = await _messageService.GetUserConversations(User.Id);
    if (response.IsSuccess)
      messages = response.Data;
  }

  public async Task MarkNotificationsAsSeen()
  {
    foreach (var notification in notifications.Where(x => !x.IsSeen))
    {
      var response = await _notificationService.SetNotificationAsSeen(notification.Id);
      if (response.IsSuccess)
        await GetNotifications();
    }
  }

  private async Task HandleImageUpload(IFileListEntry[] files)
  {
    file = files.Last();
    if (file != null)
    {
      await _imageService.UploadImage(file, $"pm-avatar-temp.jpg");
      justUploaded = true;
    }
  }

  private void Init()
  {
    if (User.Specialization is null)
      userModel.Specialization = 5;
    else
      userModel.Specialization = (int)User.Specialization;
    userModel.Email = User.Email;
    userModel.Password = string.Empty;
    userModel.PasswordToConfirm = string.Empty;
  }

  private async Task UpdateUser()
  {
    if (file != null)
    {
      var path1 = Path.Combine(env.ContentRootPath, "wwwroot/avatars", $"pm-avatar-temp.jpg");
      var path2 = Path.Combine(env.ContentRootPath, "wwwroot/avatars", $"pm-avatar-{User.Id}.jpg");
      FileInfo newFile = new FileInfo(path1);
      FileInfo oldFile = new FileInfo(path2);
      if (newFile.Exists) 
      {
        oldFile.Delete();
        System.IO.File.Move(path1, path2);
      }
    }

    if (userModel.Password == userModel.PasswordToConfirm)
    {
      bool updatePassword = userModel.Password.Length > 0;
      var userToUpdate = mapper.Map<UserSimplified>(User);
      userToUpdate.Email = userModel.Email;
      userToUpdate.Password = updatePassword ? userModel.Password : User.Password;
      userToUpdate.Specialization = User.Role == 0 || userModel.Specialization == 5 ? null : (UserSpecialization)userModel.Specialization;
      var response = await _userService.UpdateUser(userToUpdate);
      if (response.IsSuccess)
      {
        User.Email = userModel.Email;
        User.Specialization = (UserSpecialization)userModel.Specialization;
        await ShowUserSettings();
      }
    }
  }
}
