using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesajX.ChatService.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DMGroups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DMGroups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    MediaUrl = table.Column<string>(type: "text", nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsRead = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                });

            migrationBuilder.CreateTable(
                name: "TeamGroups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    TeamGroupName = table.Column<string>(type: "text", nullable: false),
                    TeamGroupPhoto = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamGroups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    GroupMemberId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<string>(type: "text", nullable: false),
                    DMGroupGroupId = table.Column<string>(type: "text", nullable: true),
                    TeamGroupGroupId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.GroupMemberId);
                    table.ForeignKey(
                        name: "FK_GroupMembers_DMGroups_DMGroupGroupId",
                        column: x => x.DMGroupGroupId,
                        principalTable: "DMGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_GroupMembers_TeamGroups_TeamGroupGroupId",
                        column: x => x.TeamGroupGroupId,
                        principalTable: "TeamGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_DMGroupGroupId",
                table: "GroupMembers",
                column: "DMGroupGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_TeamGroupGroupId",
                table: "GroupMembers",
                column: "TeamGroupGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "DMGroups");

            migrationBuilder.DropTable(
                name: "TeamGroups");
        }
    }
}
