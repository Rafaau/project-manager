﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectManager.Infrastructure.Data;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AppointmentUser", b =>
                {
                    b.Property<int>("AppointmentsId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("AppointmentsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserAppointments", (string)null);
                });

            modelBuilder.Entity("AssignmentUser", b =>
                {
                    b.Property<int>("AssignmentsId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("AssignmentsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("AssignmentUser", (string)null);
                });

            modelBuilder.Entity("ChatChannelUser", b =>
                {
                    b.Property<int>("ChatChannelsId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissedUsersId")
                        .HasColumnType("integer");

                    b.HasKey("ChatChannelsId", "PermissedUsersId");

                    b.HasIndex("PermissedUsersId");

                    b.ToTable("UserChats", (string)null);
                });

            modelBuilder.Entity("Project2User", b =>
                {
                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("ProjectsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserProjects", (string)null);
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AssignmentStageId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentStageId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.AssignmentStage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("AssignmentStages");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.ChatChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ChatChannels");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatChannelId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ChatChannelId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.InvitationLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("InvitationLinks");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsSeen")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.PrivateMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsSeen")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("integer");

                    b.Property<int>("SenderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("PrivateMessages");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Project2", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Projects2");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Role")
                        .HasColumnType("integer");

                    b.Property<int?>("Specialization")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AppointmentUser", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.Appointment", null)
                        .WithMany()
                        .HasForeignKey("AppointmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AssignmentUser", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.Assignment", null)
                        .WithMany()
                        .HasForeignKey("AssignmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChatChannelUser", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.ChatChannel", null)
                        .WithMany()
                        .HasForeignKey("ChatChannelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("PermissedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Project2User", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.Project2", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Assignment", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.AssignmentStage", "AssignmentStage")
                        .WithMany("Assignments")
                        .HasForeignKey("AssignmentStageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.Project2", "Project")
                        .WithMany("Assignments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignmentStage");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.AssignmentStage", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.Project2", "Project")
                        .WithMany("AssignmentStages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.ChatChannel", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.Project2", "Project")
                        .WithMany("ChatChannels")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.ChatMessage", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.ChatChannel", "ChatChannel")
                        .WithMany("Messages")
                        .HasForeignKey("ChatChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.Project2", "Project")
                        .WithMany("Messages")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatChannel");

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.InvitationLink", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.Project2", "Project")
                        .WithMany("InvitationLinks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Notification", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.PrivateMessage", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", "Receiver")
                        .WithMany("PrivateMessagesReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", "Sender")
                        .WithMany("PrivateMessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Project2", b =>
                {
                    b.HasOne("ProjectManager.Core.ProjectAggregate.User", "Manager")
                        .WithMany("ManagedProjects")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.AssignmentStage", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.ChatChannel", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.Project2", b =>
                {
                    b.Navigation("AssignmentStages");

                    b.Navigation("Assignments");

                    b.Navigation("ChatChannels");

                    b.Navigation("InvitationLinks");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ProjectManager.Core.ProjectAggregate.User", b =>
                {
                    b.Navigation("ManagedProjects");

                    b.Navigation("Messages");

                    b.Navigation("Notifications");

                    b.Navigation("PrivateMessagesReceived");

                    b.Navigation("PrivateMessagesSent");
                });
#pragma warning restore 612, 618
        }
    }
}
