using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class MoreJoinCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uses",
                table: "JoinCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "JoinCodeId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Creation",
                table: "JoinCodes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "JoinCodes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_JoinCodeId",
                table: "Users",
                column: "JoinCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinCodes_CreatorId",
                table: "JoinCodes",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinCodes_Users_CreatorId",
                table: "JoinCodes",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_JoinCodes_JoinCodeId",
                table: "Users",
                column: "JoinCodeId",
                principalTable: "JoinCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinCodes_Users_CreatorId",
                table: "JoinCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_JoinCodes_JoinCodeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_JoinCodeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_JoinCodes_CreatorId",
                table: "JoinCodes");

            migrationBuilder.DropColumn(
                name: "JoinCodeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Creation",
                table: "JoinCodes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "JoinCodes");

            migrationBuilder.AddColumn<int>(
                name: "Uses",
                table: "JoinCodes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
