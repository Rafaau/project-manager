using Microsoft.AspNetCore.Components;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.CalendarComponents;
public partial class AppointmentDetails
{
  [Inject] private IWebHostEnvironment env { get; set; }
  [Parameter] public AssignmentComplex? Assignment { get; set; }
  [Parameter] public AppointmentComplex? Appointment { get; set; }
}
