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
using ProjectManager.Web.ApiModels;
using Newtonsoft.Json.Bson;
using AutoMapper;
using ProjectManager.Web.DirectApiCalls.Interfaces;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Web.Pages.ProjectPages;

public partial class ProjectBoard
{
  [Inject] private NavigationManager navManager { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private IAssignmentCallService _assignmentService { get; set; }
  [Inject] private IAssignmentStageCallService _stageService { get; set; }
  [Inject] private IMapper _mapper { get; set; }
  [Inject] IWebHostEnvironment env { get; set; }
  [CascadingParameter] User User { get; set; }
  private ProjectComplex Project { get; set; }
  private AssignmentComplex[] Assignments { get; set; }
  private int Id { get; set; }
  private bool ShowAssignmentForm { get; set; } = false;
  private int CurrentStage { get; set; } = 0;
  private int SelectValue { get; set; } = 0;
  private AssignmentRequest AssignmentToAdd = new AssignmentRequest();
  private int DragAndPlaceAssignmentId { get; set; }
  private int DragAndPlaceStageId { get; set; }
  private string IsDragOver { get; set; } = "";
  private string IsDragged { get; set; } = "";
  private int ShowAssignmentDetails { get; set; } = 0;
  private bool ShowStageNameForm { get; set; } = false;
  private string StageName { get; set; }
  private int StageId { get; set; }
  private List<UserSimplified>? UnboundUsers { get; set; } = new List<UserSimplified>();
  private List<UserSimplified>? UsersToBound { get; set; } = new List<UserSimplified>();
  private int ShowUnboundButton { get; set; } = 0;

  protected override async Task OnInitializedAsync()
  {
    var uri = navManager.ToAbsoluteUri(navManager.Uri);
    if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var id))
      Id = int.Parse(id);

    await RefreshProject();
    await RefreshAssignments();
    InitUnboundUsers();
  }

  public async Task RefreshProject()
  {
    var projectResponse = await _projectService.GetById(Id);
    if (projectResponse != null)
      Project = projectResponse.Data;
  }

  public async Task RefreshAssignments()
  {
    var assignmentResponse = await _assignmentService.GetByProjectId(Id);
    if (assignmentResponse != null)
      Assignments = assignmentResponse.Data;
  }

  private void InitUnboundUsers()
  {
    UnboundUsers = Project.Users.ToList();
  }
  private void ShowForm(int stageId)
  {
    ShowAssignmentForm = true;
    CurrentStage = stageId;
    AssignmentToAdd = new AssignmentRequest();
    AssignmentToAdd.Deadline = DateTime.UtcNow;
  }

  public async Task SubmitAssignment()
  {
    AssignmentToAdd.AssignmentStageId = CurrentStage;
    AssignmentToAdd.Users = Array.Empty<UserSimplified>();
    AssignmentToAdd.ProjectId = Project.Id;
    AssignmentToAdd.Priority = (Priority)SelectValue;
    AssignmentToAdd.Users = UsersToBound!.ToArray();

    var response = await _assignmentService.AddAssignment(AssignmentToAdd);
    if (response.IsSuccess)
    {
      await RefreshProject();
      await RefreshAssignments();
      ShowAssignmentForm = false;
    }
  }

  private void OnDrag(int assignmentId)
  {
    DragAndPlaceAssignmentId = assignmentId;
  }

  private void OnDragEnter(int stageId)
  {
    DragAndPlaceStageId = stageId;
  }

  private async Task OnDrop()
  {
    var response = await _assignmentService.MoveAssignmentToStage(DragAndPlaceAssignmentId, DragAndPlaceStageId);
    DragAndPlaceStageId = 0;
    DragAndPlaceAssignmentId = 0;
    if (response != null)
    {
      await RefreshProject();
      await RefreshAssignments();
    }
  }

  private void OnAssignmentClick(int assignmentId)
  {
    if (ShowAssignmentDetails == 0 || ShowAssignmentDetails != assignmentId)
      ShowAssignmentDetails = assignmentId;
    else
      ShowAssignmentDetails = 0;
  }

  private void OnEditStageClick(AssignmentStageSimplified stage)
  {
    ShowStageNameForm = true;
    StageName = stage.Name;
    StageId = stage.Id;
  }

  private async Task SaveStageName()
  {
    ShowStageNameForm = false;
    var response = await _stageService.EditAssignmentStageName(StageId, StageName);
    if (response != null)
      await RefreshProject();
  }

  private async Task AddNewStage()
  {
    var newStage = new AssignmentStageRequest()
    {
      Index = Project.AssignmentStages.Count() + 1,
      ProjectId = Project.Id
    };

    var response = await _stageService.AddAssignmentStage(newStage);
    if (response != null)
      await RefreshProject();
  }

  private void BoundUser(UserSimplified user)
  {
    UnboundUsers.Remove(user);
    UsersToBound.Add(user);
  }

  private void UnboundUser(UserSimplified user)
  {
    UsersToBound.Remove(user);
    UnboundUsers.Add(user);
  }

  private async Task SignUpUserToAssignment(int assignmentId)
  {
    var response = await _assignmentService.SignUpUserToAssignment(assignmentId, User.Id);
    if (response.IsSuccess)
      await RefreshAssignments();
  }
}
