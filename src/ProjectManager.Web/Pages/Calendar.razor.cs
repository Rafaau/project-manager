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
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.ApiModels;
using ProjectManager.Web.Components.Modals;

namespace ProjectManager.Web.Pages;

public partial class Calendar
{
  [Inject] private IAppointmentCallService _appointmentService { get; set; }
  [Inject] private IAssignmentCallService _assignmentService { get; set; }
  [CascadingParameter] User User { get; set; }
  AddAppointment child;
  private AppointmentComplex[] appointments { get; set; }
  private AssignmentComplex[] assignments { get; set; }
  private int showAddButton { get; set; } = 0;
  public DateTime DateToPass { get; set; } = DateTime.Now;

  #region CalendarInitials
  string monthName = "";
  DateTime monthEnd;
  int monthsAway = 0;
  int numDummyColumn = 0;
  int year = 2022;
  int month = 0;
  int previousMonthEnd;
  int nextMonthStart;
  int daysAway = 0;
  DateTime currentDay = DateTime.Now.Date;
  #endregion

  protected override async Task OnInitializedAsync()
  {
    CreateMonth();
    await base.OnInitializedAsync();
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (User != null && appointments == null && assignments == null)
    {
      await GetAppointments();
      await GetAssignments();
      StateHasChanged();
    }
  }

  private void CreateMonth()
  {
    var tempDate = DateTime.Now.AddMonths(monthsAway);
    month = tempDate.Month;
    year = tempDate.Year;

    DateTime monthStart = new DateTime(year, month, 1);
    var dupa = monthStart.DayOfWeek;
    monthEnd = monthStart.AddMonths(1).AddDays(-1);
    monthName = monthStart.Month switch
    {
      1 => "January",
      2 => "February",
      3 => "March",
      4 => "April",
      5 => "May",
      6 => "June",
      7 => "July",
      8 => "August",
      9 => "September",
      10 => "October",
      11 => "November",
      12 => "December",
      _ => ""
    };

    if ((int)monthStart.DayOfWeek == 0)
      numDummyColumn = 6;
    else
      numDummyColumn = (int)monthStart.DayOfWeek - 1;
    previousMonthEnd = monthStart.AddDays(-1).Day;
    var firstDayOfNextMonth = (int)monthEnd.DayOfWeek;
    nextMonthStart = 7 - firstDayOfNextMonth;
  }

  private async Task GetAppointments()
  {
    var response = await _appointmentService.GetByUserId(User.Id);
    if (response.IsSuccess)
      appointments = response.Data; 
  }

  private async Task GetAssignments()
  {
    var response = await _assignmentService.GetByUserId(User.Id);
    if (response.IsSuccess)
      assignments = response.Data;
  }

  private void PassDateToModal(int day)
  {
    DateToPass = new DateTime(year, month, day);
    child.InitModel();
  }
}
