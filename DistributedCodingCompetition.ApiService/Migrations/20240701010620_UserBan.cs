using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class UserBan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BanId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ban",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    IssuerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ban", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ban_Users_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ban_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContestUser2",
                columns: table => new
                {
                    BannedContestsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BannedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestUser2", x => new { x.BannedContestsId, x.BannedId });
                    table.ForeignKey(
                        name: "FK_ContestUser2_Contests_BannedContestsId",
                        column: x => x.BannedContestsId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContestUser2_Users_BannedId",
                        column: x => x.BannedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ban_IssuerId",
                table: "Ban",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ban_UserId",
                table: "Ban",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContestUser2_BannedId",
                table: "ContestUser2",
                column: "BannedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ban");

            migrationBuilder.DropTable(
                name: "ContestUser2");

            migrationBuilder.DropColumn(
                name: "BanId",
                table: "Users");
        }
    }
}
