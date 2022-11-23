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

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
  public void Configure(EntityTypeBuilder<Message> modelBuilder)
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


