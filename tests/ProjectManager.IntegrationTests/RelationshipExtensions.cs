using System.Net.Http.Json;
using ProjectManager.SharedKernel;
using ProjectManager.Web.ApiModels;
using static ProjectManager.IntegrationTests.UserController.UserGenerator;
using static ProjectManager.IntegrationTests.ProjectController.ProjectGenerator;
using static ProjectManager.IntegrationTests.AssignmentStageController.AssignmentStageGenerator;
using static ProjectManager.IntegrationTests.ChatChannelController.ChatChannelGenerator;

namespace ProjectManager.IntegrationTests;
public static class RelationshipExtensions
{
  public static async Task IncludeRelationships(this AppointmentRequest appointment, HttpClient client)
  {
    var user = _userGenerator.Generate();
    var response = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    appointment.Users = new List<UserSimplified>() { userResponse!.Data }.ToArray();
  }

  public static async Task IncludeRelationships(this AssignmentRequest assignment, HttpClient client)
  {
    var stage = _stageGenerator.Generate();
    await stage.IncludeRelationships(client);
    var response = await client.PostAsJsonAsync("/api/assignmentstage", stage);
    var stageResponse = await response.Content.ReadFromJsonAsync<Response<AssignmentStageComplex>>();

    var user = _userGenerator.Generate();
    var response2 = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response2.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    assignment.ProjectId = stage.ProjectId;
    assignment.Users = new List<UserSimplified>() { userResponse!.Data }.ToArray();
    assignment.AssignmentStageId = stageResponse!.Data.Id;
  }

  public static async Task IncludeRelationships(this AssignmentStageRequest stage, HttpClient client)
  {
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(client);
    var response = await client.PostAsJsonAsync("/api/project", project);
    var projectResponse = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();
    stage.ProjectId = projectResponse!.Data.Id;
  }

  public static async Task IncludeRelationships(this ProjectRequest project, HttpClient client)
  {
    var user = _userGenerator.Generate();
    var response = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();
    project.ManagerId = userResponse!.Data.Id;
  }

  public static async Task IncludeRelationships(this ChatChannelRequest channel, HttpClient client)
  {
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(client);
    var response = await client.PostAsJsonAsync("/api/project", project);
    var projectResponse = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();

    var user = _userGenerator.Generate();
    var response2 = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response2.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    channel.ProjectId = projectResponse!.Data.Id;
    channel.PermissedUsers = new List<UserSimplified>() { userResponse!.Data }.ToArray();
  }

  public static async Task IncludeRelationships(this ChatMessageRequest message, HttpClient client)
  {
    var channel = _chatChannelGenerator.Generate();
    await channel.IncludeRelationships(client);
    var response = await client.PostAsJsonAsync("/api/chatchannel", channel);
    var channelResponse = await response.Content.ReadFromJsonAsync<Response<ChatChannelComplex>>();

    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(client);
    var response2 = await client.PostAsJsonAsync("/api/project", project);
    var projectResponse = await response2.Content.ReadFromJsonAsync<Response<ProjectComplex>>();

    var user = _userGenerator.Generate();
    var response3 = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response3.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    message.ChatChannelId = channelResponse!.Data.Id;
    message.ProjectId = projectResponse!.Data.Id;
    message.UserId = userResponse!.Data.Id;
  }

  public static async Task IncludeRelationships(this InvitationLinkRequest invitationLink, HttpClient client)
  {
    var project = _projectGenerator.Generate();
    await project.IncludeRelationships(client);
    var response = await client.PostAsJsonAsync("/api/project", project);
    var projectResponse = await response.Content.ReadFromJsonAsync<Response<ProjectComplex>>();

    invitationLink.ProjectId = projectResponse!.Data.Id;
  }

  public static async Task IncludeRelationships(this NotificationRequest notification, HttpClient client)
  {
    var user = _userGenerator.Generate();
    var response = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    notification.UserId = userResponse!.Data.Id;
  }

  public static async Task IncludeRelationships(this PrivateMessageRequest message, HttpClient client)
  {
    var user = _userGenerator.Generate();
    var response = await client.PostAsJsonAsync("/api/user", user);
    var userResponse = await response.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    var user2 = _userGenerator.Generate();
    var response2 = await client.PostAsJsonAsync("/api/user", user2);
    var userResponse2 = await response2.Content.ReadFromJsonAsync<Response<UserSimplified>>();

    message.SenderId = userResponse!.Data.Id;
    message.ReceiverId = userResponse2!.Data.Id;
  }
}
