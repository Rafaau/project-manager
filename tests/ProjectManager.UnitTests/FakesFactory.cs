using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.UnitTests;
public static class FakesFactory
{
  public static Assignment FakeAssignment()
  {
    return new Assignment()
    {
      Id = 1,
      Name = "Test",
      Description = "Test",
      ProjectId = 1,
      Project = FakeProject(),
      AssignmentStageId = 1,
      AssignmentStage = FakeStage(),
      Users = new List<User>(),
      Priority = (Priority)2,
      Deadline = new DateTime(2026, 6, 17),
    };
  }

  public static List<Assignment> FakeAssignmentsList()
  {
    return new List<Assignment>()
    {
      FakeAssignment()
    };
  }

  public static Project2 FakeProject()
  {
    return new Project2()
    {
      Id = 1,
      Name = "Test",
      ManagerId = 1,
      Manager = FakeUser(),
      Users = new List<User>(),
      Assignments = new List<Assignment>(),
      Messages = new List<ChatMessage>()
    };
  }

  public static List<Project2> FakeProjectsList()
  {
    return new List<Project2>()
    {
      FakeProject()
    };
  }

  public static ChatMessage FakeMessage()
  {
    return new ChatMessage()
    {
      Id = 1,
      Content = "Test",
      UserId = 1,
      User = new User(),
      ProjectId = 1,
      Project = FakeProject(),
      PostDate = new DateTime(2026, 6, 17),
    };
  }

  public static List<ChatMessage> FakeMessageList()
  {
    return new List<ChatMessage>()
    {
      FakeMessage()
    };
  }

  public static User FakeUser()
  {
    return new User()
    {
      Id = 1,
      Firstname = "Test",
      Lastname = "Test",
      Email = "Test",
      Password = "Test",
      Role = (UserRole)0
    };
  }

  public static List<User> FakeUsersList()
  {
    return new List<User>()
    {
      FakeUser()
    };
  }

  public static AssignmentStage FakeStage()
  {
    return new AssignmentStage()
    {
      Id = 1,
      Name = "Test",
      ProjectId = 1,
      Index = 1,
      Assignments = new List<Assignment>(),
      Project = FakeProject(),
    };
  }

  public static List<AssignmentStage> FakeStagesList()
  {
    return new List<AssignmentStage>()
    {
      FakeStage()
    };
  }

  public static Appointment FakeAppointment()
  {
    return new Appointment()
    {
      Id = 1,
      Name = "Test",
      Description = "Test",
      Date = new DateTime(2026, 6, 17),
      Users = FakeUsersList()
    };
  }

  public static List<Appointment> FakeAppointmentsList()
  {
    return new List<Appointment>()
    {
      FakeAppointment()
    };
  }

  public static ChatChannel FakeChatChannel()
  {
    return new ChatChannel()
    {
      Id = 1,
      ProjectId = 1,
      Project = FakeProject(),
      PermissedUsers = FakeUsersList(),
      Messages = FakeMessageList()
    };
  }

  public static InvitationLink FakeInvitationLink()
  {
    return new InvitationLink()
    {
      Id = 1,
      ProjectId = 1,
      Url = "Test",
      IsUsed = false,
      ExpirationTime = DateTime.UtcNow
    };
  }

  public static Notification FakeNotification()
  {
    return new Notification()
    {
      Id = 1,
      User = FakeUser(),
      Content = "Test",
      IsSeen = false,
      Date = DateTime.UtcNow
    };
  }

  public static List<Notification> FakeNotificationsList()
  {
    return new List<Notification>()
    {
      FakeNotification()
    };
  }

  public static PrivateMessage FakePrivateMessage()
  {
    return new PrivateMessage()
    {
      Id = 1,
      ReceiverId = 1,
      SenderId = 2,
      Content = "Test",
      PostDate = DateTime.UtcNow,
      IsSeen = false
    };
  }

  public static List<PrivateMessage> FakePrivateMessagesList()
  {
    return new List<PrivateMessage>()
    {
      FakePrivateMessage()
    };
  }
}
