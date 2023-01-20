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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.Interfaces;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.Modals;

public partial class AddProject
{
  [CascadingParameter] User User { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Parameter] public EventCallback UpdateProjectList { get; set; }
  [Inject] private IJSRuntime _js { get; set; }
  private class Model
  {
    public string Name { get; set; }
  }

  private Model model = new Model();

  private async Task Submit()
  {
    var project = new ProjectRequest()
    {
      Name = model.Name,
      ManagerId = User.Id
    };

    var response = await _projectService.AddProject(project);
    if (response.IsSuccess)
    {
      await _js.InvokeVoidAsync("CloseModal", "#addProject");
      await UpdateProjectList.InvokeAsync();
    }
  }
}
