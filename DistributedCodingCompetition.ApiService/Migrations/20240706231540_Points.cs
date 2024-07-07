using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class Points : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Submissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "Submissions");
        }
    }
}
