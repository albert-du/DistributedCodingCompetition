using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ContestDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumAge",
                table: "Contests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Open",
                table: "Contests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumAge",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "Open",
                table: "Contests");
        }
    }
}
