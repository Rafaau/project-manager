using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Core.ProjectAggregate.Enums;

namespace ProjectManager.Infrastructure.Data.Config;
public static class SeedExtensions
{
  public static void SeedUsers(this EntityTypeBuilder<User> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      User1,
      User2,
      User3,
      User4,
      User5
    );
  }

  public static void SeedProjects(this EntityTypeBuilder<Project2> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new Project2 { Id = 1, Name = "Project Manager", ManagerId = 1 },
      new Project2 { Id = 2, Name = "Digital Library", ManagerId = 1 },
      new Project2 { Id = 3, Name = "Office Management", ManagerId = 1 }
    );
  }

  public static void SeedAssignments(this EntityTypeBuilder<Assignment> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new Assignment { Id = 1, ProjectId = 1, AssignmentStageId = 1, Name = "Refactor services", Description = "Services are broken and need to be optimized.", Deadline = DateTime.UtcNow.AddDays(4), Priority = Priority.High },
      new Assignment { Id = 2, ProjectId = 1, AssignmentStageId = 1, Name = "Frontend bugs", Description = "There are some problems with components that need to be fixed.", Deadline = DateTime.UtcNow.AddDays(6), Priority = Priority.High }
    );
  }

  public static void SeedChatMessages(this EntityTypeBuilder<ChatMessage> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new ChatMessage { Id = 1, ProjectId = 1, ChatChannelId = 1, UserId = 2, PostDate = DateTime.UtcNow.AddDays(-2), Content = "This is test message @Kadie Rooney @Noriah Craig" },
      new ChatMessage { Id = 2, ProjectId = 1, ChatChannelId = 1, UserId = 2, PostDate = DateTime.UtcNow.AddDays(-2), Content = "This is test message." }
    );
  }

  public static void SeedAssignmentStages(this EntityTypeBuilder<AssignmentStage> entityTypeBuilder) 
  {
    entityTypeBuilder.HasData(
      new AssignmentStage { Id = 1, ProjectId = 1, Name = "To Do", Index = 1 },
      new AssignmentStage { Id = 2, ProjectId = 1, Name = "In Progress", Index = 3 },
      new AssignmentStage { Id = 3, ProjectId = 1, Name = "Done", Index = 2 }
    );
  }

  public static void SeedChatChannels(this EntityTypeBuilder<ChatChannel> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new ChatChannel { Id = 1, Name = "General", PermissedUsers = null, ProjectId = 1 },
      new ChatChannel { Id = 2, Name = "Daily", PermissedUsers = null, ProjectId = 1 },
      new ChatChannel { Id = 3, Name = "General", PermissedUsers = null, ProjectId = 2 },
      new ChatChannel { Id = 4, Name = "Daily", PermissedUsers = null, ProjectId = 2 },
      new ChatChannel { Id = 5, Name = "General", PermissedUsers = null, ProjectId = 3 },
      new ChatChannel { Id = 6, Name = "Daily", PermissedUsers = null, ProjectId = 3 }
    );
  }

  public static void SeedAppointments(this EntityTypeBuilder<Appointment> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      Appointment1,
      Appointment2,
      Appointment3
    );
  }

  public static void SeedNotifications(this EntityTypeBuilder<Notification> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new Notification { Id = 1, UserId = 1, Content = "You have been mentioned in Project Manager #General chat.", Date = DateTime.UtcNow.AddDays(-1), IsSeen = false },
      new Notification { Id = 2, UserId = 1, Content = "Hugh Campbell has moved the assignment to the next stage.", Date = DateTime.UtcNow.AddDays(-1), IsSeen = false }
    );
  }

  public static void SeedPrivateMessages(this EntityTypeBuilder<PrivateMessage> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new PrivateMessage { Id = 1, SenderId = 2, ReceiverId = 2, Content = "This is test message.", IsSeen = true, PostDate = DateTime.UtcNow.AddDays(-1).AddMinutes(-30) },
      new PrivateMessage { Id = 2, SenderId = 1, ReceiverId = 2, Content = "This is test message.", IsSeen = false, PostDate = DateTime.UtcNow.AddDays(-3).AddMinutes(-40) },
      new PrivateMessage { Id = 3, SenderId = 3, ReceiverId = 1, Content = "This is test message.", IsSeen = false, PostDate = DateTime.UtcNow.AddDays(-4).AddMinutes(-50) },
      new PrivateMessage { Id = 4, SenderId = 4, ReceiverId = 1, Content = "This is test message.", IsSeen = false, PostDate = DateTime.UtcNow.AddDays(-2).AddMinutes(-20) }
    );
  }

  public static void SeedAppointmentsUsers(this EntityTypeBuilder<Dictionary<string, object>> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new { AppointmentsId = 1, UsersId = 1 },
      new { AppointmentsId = 1, UsersId = 2 },
      new { AppointmentsId = 1, UsersId = 3 },
      new { AppointmentsId = 1, UsersId = 4 },
      new { AppointmentsId = 2, UsersId = 1 },
      new { AppointmentsId = 2, UsersId = 2 },
      new { AppointmentsId = 2, UsersId = 3 },
      new { AppointmentsId = 2, UsersId = 4 },
      new { AppointmentsId = 3, UsersId = 1 },
      new { AppointmentsId = 3, UsersId = 2 },
      new { AppointmentsId = 3, UsersId = 3 },
      new { AppointmentsId = 3, UsersId = 4 }
    );
  }

  public static void SeedAssignmentsUsers(this EntityTypeBuilder<Dictionary<string, object>> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new { AssignmentsId = 1, UsersId = 1 },
      new { AssignmentsId = 1, UsersId = 2 },
      new { AssignmentsId = 1, UsersId = 3 },
      new { AssignmentsId = 1, UsersId = 4 },
      new { AssignmentsId = 2, UsersId = 1 },
      new { AssignmentsId = 2, UsersId = 2 },
      new { AssignmentsId = 2, UsersId = 3 },
      new { AssignmentsId = 2, UsersId = 4 }
    );
  }

  public static void SeedProjectsUsers(this EntityTypeBuilder<Dictionary<string, object>> entityTypeBuilder)
  {
    entityTypeBuilder.HasData(
      new { ProjectsId = 1, UsersId = 2 },
      new { ProjectsId = 1, UsersId = 3 },
      new { ProjectsId = 1, UsersId = 4 },
      new { ProjectsId = 2, UsersId = 2 },
      new { ProjectsId = 2, UsersId = 3 },
      new { ProjectsId = 2, UsersId = 4 },
      new { ProjectsId = 3, UsersId = 2 },
      new { ProjectsId = 3, UsersId = 3 },
      new { ProjectsId = 3, UsersId = 4 }
    );
  }

  static User User1 = new User { Id = 1, Firstname = "Kadie", Lastname = "Rooney", Email = "rooney@gmail.com", Password = "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", Role = UserRole.Manager, Appointments = Appointments };
  static User User2 = new User { Id = 2, Firstname = "Hugh", Lastname = "Campbell", Email = "campbell@gmail.com", Password = "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", Role = UserRole.Developer, Appointments = Appointments };
  static User User3 = new User { Id = 3, Firstname = "Noriah", Lastname = "Craig", Email = "craig@gmail.com", Password = "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", Role = UserRole.Developer, Appointments = Appointments };
  static User User4 = new User { Id = 4, Firstname = "Arthur", Lastname = "Morgan", Email = "morgan@gmail.com", Password = "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", Role = UserRole.Developer, Appointments = Appointments };
  static User User5 = new User { Id = 5, Firstname = "Abigail", Lastname = "Murray", Email = "murray@gmail.com", Password = "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", Role = UserRole.Developer };
  static List<User> ProjectMembers = new List<User>() { User2, User3, User4 };
  static List<User> AssignmentMembers = new List<User>() { User1, User2, User3, User4 };
  static Appointment Appointment1 = new Appointment { Id = 1, Name = "Weekly", Description = "As in title", Date = DateTime.UtcNow.AddDays(3) };
  static Appointment Appointment2 = new Appointment { Id = 2, Name = "Demo presentation", Description = "For customers", Date = DateTime.UtcNow.AddDays(5) };
  static Appointment Appointment3 = new Appointment { Id = 3, Name = "Brainstorming", Description = "On Teams video conference", Date = DateTime.UtcNow.AddDays(5) };
  static List<Appointment> Appointments = new List<Appointment>() { Appointment1, Appointment2, Appointment3 };
}
