using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    public partial class tablenamesfixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentStages_Projects2_ProjectId",
                table: "AssignmentStages");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Projects2_ProjectId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatChannels_Projects2_ProjectId",
                table: "ChatChannels");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatChannels_ChatChannelId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Projects2_ProjectId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_InvitationLinks_Projects2_ProjectId",
                table: "InvitationLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects2_Users_ManagerId",
                table: "Projects2");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_Projects2_ProjectsId",
                table: "UserProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects2",
                table: "Projects2");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "ChatMessages");

            migrationBuilder.RenameTable(
                name: "Projects2",
                newName: "Projects");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ProjectId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatChannelId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ChatChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects2_ManagerId",
                table: "Projects",
                newName: "IX_Projects_ManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentStages_Projects_ProjectId",
                table: "AssignmentStages",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Projects_ProjectId",
                table: "Assignments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatChannels_Projects_ProjectId",
                table: "ChatChannels",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatChannels_ChatChannelId",
                table: "ChatMessages",
                column: "ChatChannelId",
                principalTable: "ChatChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Projects_ProjectId",
                table: "ChatMessages",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_UserId",
                table: "ChatMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationLinks_Projects_ProjectId",
                table: "InvitationLinks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_Projects_ProjectsId",
                table: "UserProjects",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentStages_Projects_ProjectId",
                table: "AssignmentStages");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Projects_ProjectId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatChannels_Projects_ProjectId",
                table: "ChatChannels");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatChannels_ChatChannelId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Projects_ProjectId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_UserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_InvitationLinks_Projects_ProjectId",
                table: "InvitationLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjects_Projects_ProjectsId",
                table: "UserProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects2");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_UserId",
                table: "Messages",
                newName: "IX_Messages_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ProjectId",
                table: "Messages",
                newName: "IX_Messages_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ChatChannelId",
                table: "Messages",
                newName: "IX_Messages_ChatChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects2",
                newName: "IX_Projects2_ManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects2",
                table: "Projects2",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentStages_Projects2_ProjectId",
                table: "AssignmentStages",
                column: "ProjectId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Projects2_ProjectId",
                table: "Assignments",
                column: "ProjectId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatChannels_Projects2_ProjectId",
                table: "ChatChannels",
                column: "ProjectId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatChannels_ChatChannelId",
                table: "Messages",
                column: "ChatChannelId",
                principalTable: "ChatChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Projects2_ProjectId",
                table: "Messages",
                column: "ProjectId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvitationLinks_Projects2_ProjectId",
                table: "InvitationLinks",
                column: "ProjectId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects2_Users_ManagerId",
                table: "Projects2",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjects_Projects2_ProjectsId",
                table: "UserProjects",
                column: "ProjectsId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
