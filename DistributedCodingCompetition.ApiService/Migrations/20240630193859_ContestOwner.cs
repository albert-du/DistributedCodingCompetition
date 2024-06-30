using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ContestOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Contests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contests_OwnerId",
                table: "Contests",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contests_Users_OwnerId",
                table: "Contests",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contests_Users_OwnerId",
                table: "Contests");

            migrationBuilder.DropIndex(
                name: "IX_Contests_OwnerId",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Contests");
        }
    }
}
