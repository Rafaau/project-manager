using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      Messages = new List<Message>()
    };
  }

  public static List<Project2> FakeProjectsList()
  {
    return new List<Project2>()
    {
      FakeProject()
    };
  }

  public static Message FakeMessage()
  {
    return new Message()
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

  public static List<Message> FakeMessageList()
  {
    return new List<Message>()
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
    };
  }

  public static List<AssignmentStage> FakeStagesList()
  {
    return new List<AssignmentStage>()
    {
      FakeStage()
    };
  }
}
