using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Core.ProjectAggregate.Enums;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;

namespace ProjectManager.Core.ProjectAggregate;
public class User : EntityBase, IAggregateRoot
{
  public string Firstname { get; set; } = string.Empty;
  public string Lastname { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public UserRole? Role { get; set; }
  public virtual ICollection<Project2>? ManagedProjects { get; set; }
  public virtual ICollection<Project2>? Projects { get; set; }
  public UserSpecialization? Specialization { get; set; }
  public virtual ICollection<Assignment>? Assignments { get; set; }
  public virtual ICollection<ChatMessage>? Messages { get; set; }
  public virtual ICollection<ChatChannel>? ChatChannels { get; set; }
  public virtual ICollection<Appointment>? Appointments { get; set; }
  public virtual ICollection<Notification>? Notifications { get; set; }
  public virtual ICollection<PrivateMessage>? PrivateMessagesSent { get; set; }
  public virtual ICollection<PrivateMessage>? PrivateMessagesReceived { get; set; }

  public User(string firstname, string lastname, string email, string password, UserRole role, ICollection<Project2>? managedProjects, ICollection<Project2>? projects)
  {
    Firstname = firstname;
    Lastname = lastname;
    Email = email;
    Role = role;
    ManagedProjects = managedProjects;
    Projects = projects;
    Password = password;
  }

  public User()
  {
  }
}
