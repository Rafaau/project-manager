namespace ProjectManager.Web.Shared;
public partial class SideNav
{
  private bool collapseNavMenu = true;
  private string? SideNavCssClass => collapseNavMenu ? "collapse" : null;
  private void ToggleNavMenu()
  {
    collapseNavMenu = !collapseNavMenu;
  }
}
