using Microsoft.AspNetCore.Components;

namespace ProjectManager.Web.Components.DashboardComponents;

public partial class UserSettings
{
  [Parameter] public EventCallback onSaveClick { get; set; }

  public async Task SaveProfile()
  {
    await onSaveClick.InvokeAsync();
  }
}
