using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Core.ProjectAggregate;

namespace ProjectManager.Infrastructure.Data.Config;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> modelBuilder)
  {
    modelBuilder.HasMany(u => u.Projects)
                .WithMany(p => p.Users)
                .UsingEntity(j => j.ToTable("UserProjects"));
  }
}

public class ProjectConfiguration : IEntityTypeConfiguration<Project2>
{
  public void Configure(EntityTypeBuilder<Project2> modelBuilder)
  {
    modelBuilder.HasOne(p => p.Manager)
                .WithMany(u => u.ManagedProjects)
                .HasForeignKey(p => p.ManagerId);
  }
}

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
  public void Configure(EntityTypeBuilder<Assignment> modelBuilder)
  {
    modelBuilder.HasOne(a => a.Project)
                .WithMany(p => p.Assignments)
                .HasForeignKey(a => a.ProjectId);

    modelBuilder.HasMany(a => a.Users)
                .WithMany(u => u.Assignments)
                .UsingEntity(j => j.ToTable("AssignmentUser"));
  }
}

public class MessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
  public void Configure(EntityTypeBuilder<ChatMessage> modelBuilder)
  {
    modelBuilder.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.UserId);

    modelBuilder.HasOne(m => m.Project)
                .WithMany(p => p.Messages)
                .HasForeignKey(m => m.ProjectId);

    modelBuilder.HasOne(m => m.ChatChannel)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatChannelId);
  }
}

public class AssignmentStageConfiguration : IEntityTypeConfiguration<AssignmentStage>
{
  public void Configure(EntityTypeBuilder<AssignmentStage> modelBuilder)
  {
    modelBuilder.HasOne(m => m.Project)
                .WithMany(u => u.AssignmentStages)
                .HasForeignKey(m => m.ProjectId);

    modelBuilder.HasMany(m => m.Assignments)
                .WithOne(p => p.AssignmentStage)
                .HasForeignKey(m => m.AssignmentStageId);
  }
}

public class ChatChannelConfiguration : IEntityTypeConfiguration<ChatChannel>
{
  public void Configure(EntityTypeBuilder<ChatChannel> modelBuilder)
  {
    modelBuilder.HasMany(c => c.PermissedUsers)
                .WithMany(u => u.ChatChannels)
                .UsingEntity(j => j.ToTable("UserChats"));

    modelBuilder.HasOne(c => c.Project)
                .WithMany(p => p.ChatChannels)
                .HasForeignKey(c => c.ProjectId);
  }
}

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
  public void Configure(EntityTypeBuilder<Appointment> modelBuilder)
  {
    modelBuilder.HasMany(a => a.Users)
                .WithMany(u => u.Appointments)
                .UsingEntity(j => j.ToTable("UserAppointments"));
  }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
  public void Configure(EntityTypeBuilder<Notification> modelBuilder)
  {
    modelBuilder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);
  }
}

public class PrivateMessageConfiguration : IEntityTypeConfiguration<PrivateMessage>
{
  public void Configure(EntityTypeBuilder<PrivateMessage> modelBuilder)
  {
    modelBuilder.HasOne(p => p.Sender)
                .WithMany(u => u.PrivateMessagesSent)
                .HasForeignKey(p => p.SenderId);

    modelBuilder.HasOne(p => p.Receiver)
                .WithMany(u => u.PrivateMessagesReceived)
                .HasForeignKey(p => p.ReceiverId);
  }
}

public class InvitationLinkConfiguration : IEntityTypeConfiguration<InvitationLink>
{
  public void Configure(EntityTypeBuilder<InvitationLink> modelBuilder)
  {
    modelBuilder.HasOne(i => i.Project)
                .WithMany(p => p.InvitationLinks)
                .HasForeignKey(i => i.ProjectId);
  }
}


