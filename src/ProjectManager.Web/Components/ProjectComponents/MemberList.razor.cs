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
using Microsoft.AspNetCore.WebUtilities;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;
using AutoMapper;

namespace ProjectManager.Web.Components.ProjectComponents;

public partial class MemberList
{
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private NavigationManager navManager { get; set; }
  [Inject] private IWebHostEnvironment env { get; set; }
  [Inject] private IMapper _mapper { get; set; }
  private int Id { get; set; }
  private UserDetails details;

  private ProjectComplex Project { get; set; }

  protected override async Task OnInitializedAsync()
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
      Id = int.Parse(id);
    var projectResponse = await _projectService.GetById(Id);
    if (projectResponse != null)
      Project = projectResponse.Data;
  }
}
