using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProjectManager.Core.Interfaces;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.Authentication;

namespace ProjectManager.Web.Shared;

public partial class MainLayout
{
  [Inject] private NavigationManager navManager { get; set; }
}
