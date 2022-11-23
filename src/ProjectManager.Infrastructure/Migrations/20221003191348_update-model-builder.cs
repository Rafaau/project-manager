using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    public partial class updatemodelbuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUser_Users_UserId",
                table: "AssignmentUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AssignmentUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentUser_UserId",
                table: "AssignmentUser",
                newName: "IX_AssignmentUser_UsersId");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ProjectId",
                table: "Messages",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUser_Users_UsersId",
                table: "AssignmentUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Projects2_ProjectId",
                table: "Messages",
                column: "ProjectId",
                principalTable: "Projects2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentUser_Users_UsersId",
                table: "AssignmentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Projects2_ProjectId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ProjectId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "AssignmentUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AssignmentUser_UsersId",
                table: "AssignmentUser",
                newName: "IX_AssignmentUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentUser_Users_UserId",
                table: "AssignmentUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
