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
using ProjectManager.Web.Api;
using ProjectManager.Core.Interfaces;
using ProjectManager.Web.Authentication;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Enums;

namespace ProjectManager.Web.Pages;

public partial class Register
{
  [Inject] private IUserCallService _userService { get; set; }
  [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  [Inject] private IJSRuntime js { get; set; }
  private string View { get; set; } = "form";

  private class Model
  {
    public string  Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
  }

  private Model model = new Model();

  private async Task Submit()
  {
    var user = new User
    (
      firstname: model.Firstname,
      lastname: model.Lastname,
      email: model.Email,
      password: model.Password.HashPassword(),
      role: (UserRole)model.Role,
      managedProjects: new List<Project2>(),
      projects: new List<Project2>()
    );

    var response = await _userService.AddUser(user);
    if (response.IsSuccess)
      View = "success";
    else
      View = "failure";
  }
  
  private void RedirectToLogin()
  {
    navManager.NavigateTo("/login");
  }

  private void BackToFormView()
  {
    View = "form";
  }
}
