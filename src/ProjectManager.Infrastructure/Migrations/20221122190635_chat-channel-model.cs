using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectManager.Infrastructure.Migrations
{
    public partial class chatchannelmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatChannelId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatChannels_Projects2_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChats",
                columns: table => new
                {
                    ChatChannelsId = table.Column<int>(type: "integer", nullable: false),
                    PermissedUsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChats", x => new { x.ChatChannelsId, x.PermissedUsersId });
                    table.ForeignKey(
                        name: "FK_UserChats_ChatChannels_ChatChannelsId",
                        column: x => x.ChatChannelsId,
                        principalTable: "ChatChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChats_Users_PermissedUsersId",
                        column: x => x.PermissedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatChannelId",
                table: "Messages",
                column: "ChatChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatChannels_ProjectId",
                table: "ChatChannels",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_PermissedUsersId",
                table: "UserChats",
                column: "PermissedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ChatChannels_ChatChannelId",
                table: "Messages",
                column: "ChatChannelId",
                principalTable: "ChatChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ChatChannels_ChatChannelId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "UserChats");

            migrationBuilder.DropTable(
                name: "ChatChannels");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatChannelId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatChannelId",
                table: "Messages");
        }
    }
}
