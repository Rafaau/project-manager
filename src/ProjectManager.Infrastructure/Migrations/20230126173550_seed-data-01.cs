using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    public partial class seeddata01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "Date", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 29, 17, 35, 49, 701, DateTimeKind.Utc).AddTicks(8364), "As in title", "Weekly" },
                    { 2, new DateTime(2023, 1, 31, 17, 35, 49, 701, DateTimeKind.Utc).AddTicks(8519), "For customers", "Demo presentation" },
                    { 3, new DateTime(2023, 1, 31, 17, 35, 49, 701, DateTimeKind.Utc).AddTicks(8521), "On Teams video conference", "Brainstorming" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Firstname", "Lastname", "Password", "Role", "Specialization" },
                values: new object[,]
                {
                    { 1, "rooney@gmail.com", "Kadie", "Rooney", "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", 0, null },
                    { 2, "campbell@gmail.com", "Hugh", "Campbell", "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", 1, null },
                    { 3, "craig@gmail.com", "Noriah", "Craig", "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", 1, null },
                    { 4, "morgan@gmail.com", "Arthur", "Morgan", "AKSjnVFBcTQXExt0sCHkfHmPXzIvUs/bgfk408knGZeX+E1X4aWeXqlmunzw306kkA==", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Content", "Date", "IsSeen", "UserId" },
                values: new object[,]
                {
                    { 1, "You have been mentioned in Project Manager #General chat.", new DateTime(2023, 1, 25, 17, 35, 49, 964, DateTimeKind.Utc).AddTicks(4376), false, 1 },
                    { 2, "Hugh Campbell has moved the assignment to the next stage.", new DateTime(2023, 1, 25, 17, 35, 49, 964, DateTimeKind.Utc).AddTicks(4377), false, 1 }
                });

            migrationBuilder.InsertData(
                table: "PrivateMessages",
                columns: new[] { "Id", "Content", "IsSeen", "PostDate", "ReceiverId", "SenderId" },
                values: new object[,]
                {
                    { 1, "This is test message.", true, new DateTime(2023, 1, 25, 17, 5, 49, 965, DateTimeKind.Utc).AddTicks(3840), 2, 2 },
                    { 2, "This is test message.", false, new DateTime(2023, 1, 23, 16, 55, 49, 965, DateTimeKind.Utc).AddTicks(3842), 2, 1 },
                    { 3, "This is test message.", false, new DateTime(2023, 1, 22, 16, 45, 49, 965, DateTimeKind.Utc).AddTicks(3843), 1, 3 },
                    { 4, "This is test message.", false, new DateTime(2023, 1, 24, 17, 15, 49, 965, DateTimeKind.Utc).AddTicks(3844), 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ManagerId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Project Manager" },
                    { 2, 1, "Digital Library" },
                    { 3, 1, "Office Management" }
                });

            migrationBuilder.InsertData(
                table: "UserAppointments",
                columns: new[] { "AppointmentsId", "UsersId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 3 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "AssignmentStages",
                columns: new[] { "Id", "Index", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 1, 1, "To Do", 1 },
                    { 2, 3, "In Progress", 1 },
                    { 3, 2, "Done", 1 }
                });

            migrationBuilder.InsertData(
                table: "ChatChannels",
                columns: new[] { "Id", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 1, "General", 1 },
                    { 2, "Daily", 1 },
                    { 3, "General", 2 },
                    { 4, "Daily", 2 },
                    { 5, "General", 3 },
                    { 6, "Daily", 3 }
                });

            migrationBuilder.InsertData(
                table: "UserProjects",
                columns: new[] { "ProjectsId", "UsersId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 2 },
                    { 3, 3 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "AssignmentStageId", "Deadline", "Description", "Name", "Priority", "ProjectId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 1, 30, 17, 35, 49, 964, DateTimeKind.Utc).AddTicks(1367), "Services are broken and need to be optimized.", "Refactor services", 2, 1 },
                    { 2, 1, new DateTime(2023, 2, 1, 17, 35, 49, 964, DateTimeKind.Utc).AddTicks(1373), "There are some problems with components that need to be fixed.", "Frontend bugs", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "ChatChannelId", "Content", "PostDate", "ProjectId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "This is test message @Kadie Rooney @Noriah Craig", new DateTime(2023, 1, 24, 17, 35, 49, 964, DateTimeKind.Utc).AddTicks(3652), 1, 2 },
                    { 2, 1, "This is test message.", new DateTime(2023, 1, 24, 17, 35, 49, 964, DateTimeKind.Utc).AddTicks(3654), 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "AssignmentUser",
                columns: new[] { "AssignmentsId", "UsersId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AssignmentStages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AssignmentStages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "AssignmentUser",
                keyColumns: new[] { "AssignmentsId", "UsersId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "ChatChannels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ChatChannels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ChatChannels",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ChatChannels",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ChatChannels",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PrivateMessages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PrivateMessages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PrivateMessages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PrivateMessages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UserAppointments",
                keyColumns: new[] { "AppointmentsId", "UsersId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UserProjects",
                keyColumns: new[] { "ProjectsId", "UsersId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ChatChannels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AssignmentStages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
