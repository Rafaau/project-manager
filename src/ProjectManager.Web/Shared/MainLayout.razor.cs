using Microsoft.AspNetCore.Components;

namespace ProjectManager.Web.Shared;

public partial class MainLayout
{
  [Inject] private NavigationManager navManager { get; set; }
}
