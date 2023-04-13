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
using BlazorInputFile;
using System.IO;
using ProjectManager.Web.ApiModels;

namespace ProjectManager.Web.Components.ProjectComponents;

public partial class MessageContent
{
  [Parameter] public List<string> MembersFullnames { get; set; }
  [Parameter] public ChatMessageComplex Message { get; set; }
  [Parameter] public UserSimplified[] UsersToMention { get; set; }
  private UserDetails details;
}
