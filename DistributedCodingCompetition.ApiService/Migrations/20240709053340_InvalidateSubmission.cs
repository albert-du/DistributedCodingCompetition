using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class InvalidateSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Invalidated",
                table: "Submissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invalidated",
                table: "Submissions");
        }
    }
}
