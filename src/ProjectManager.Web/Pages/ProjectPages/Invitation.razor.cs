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
using BlazorInputFile;
using System.IO;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using ProjectManager.Web.ApiModels;
using ProjectManager.Core.ProjectAggregate;
using AutoMapper;

namespace ProjectManager.Web.Pages;

public partial class Invitation
{
  [CascadingParameter] User User { get; set; }
  [Inject] private NavigationManager _navManager { get; set; }
  [Inject] private IInvitationLinkCallService _invitationService { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private IWebHostEnvironment env { get; set; }
  [Inject] private IMapper _mapper { get; set; }
  private InvitationLinkComplex invitationLink { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var uri = _navManager.ToAbsoluteUri(_navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("url", out var url))
    {
      var invitationResponse = await _invitationService.GetInvitationLink(url);
      if (invitationResponse.IsSuccess)
      {
        invitationLink = invitationResponse.Data;
      }
    }
  }

  private async Task HandleJoinClick()
  {
    var projectResponse = await _projectService.GetById(invitationLink.ProjectId);
    if (projectResponse.IsSuccess)
    {
      var projectToUpdate = projectResponse.Data;
      var usersList = projectToUpdate.Users.ToList();
      usersList.Add(_mapper.Map<UserSimplified>(User));
      projectToUpdate.Users = usersList.ToArray();
      var projectUpdateResponse = await _projectService.UpdateProject(projectToUpdate);
      if (projectUpdateResponse.IsSuccess)
      {
        var invitationResponse = await _invitationService.SetInvitationLinkAsUsed(invitationLink.Id);
        if (invitationResponse.IsSuccess)
        {
          _navManager.NavigateTo($"/project/general?id={invitationLink.ProjectId}&");
        }
      }
    }

  }
}
