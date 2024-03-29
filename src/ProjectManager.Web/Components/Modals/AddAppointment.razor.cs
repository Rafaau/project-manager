﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.DirectApiCalls.Interfaces;

namespace ProjectManager.Web.Components.Modals;

public partial class AddAppointment
{
  [Inject] private IAppointmentCallService _appointmentService { get; set; }
  [Inject] private IProjectCallService _projectService { get; set; }
  [Inject] private IJSRuntime _js { get; set; }
  [Inject] private IWebHostEnvironment env { get; set; }
  [Parameter] public DateTime Date { get; set; }
  [CascadingParameter] public User User { get; set; }
  [Parameter] public EventCallback onAppointmentCreate { get; set; }
  private AppointmentRequest appointmentReq { get; set; }
  private ProjectComplex[] userProjects { get; set; }
  private List<UserSimplified> participants { get; set; }
  private UserSimplified ParticipantToRemove { get; set; } = null;

  protected override async Task OnInitializedAsync()
  {
    InitModel();
    var response = await _projectService.GetUserProjects(User.Id);
    if (response.IsSuccess)
      userProjects = response.Data;
  }

  public void InitModel()
  {
    appointmentReq = new AppointmentRequest();
    var currentUser = new UserSimplified(User.Id, User.Firstname, User.Lastname);
    participants = new List<UserSimplified>();
    participants.Add(currentUser);
  }

  private void AddParticipantsByProject(ProjectComplex project)
  {
    foreach (var user in project.Users)
    {
      if (participants.Any(u => u.Id == user.Id))
        continue;
      participants.Add(user);
    }
  }

  private void RemoveParticipant(UserSimplified user)
  {
    participants.Remove(user);
  }

  private async Task Submit()
  {
    appointmentReq.Users = participants.ToArray();
    appointmentReq.Date = Date.AddDays(1).ToUniversalTime();
    var response = await _appointmentService.CreateAppointment(appointmentReq);
    if (response.IsSuccess)
    {
      await _js.InvokeVoidAsync("CloseModal", "#addAppointment");
      await onAppointmentCreate.InvokeAsync();
    }
  }
}
