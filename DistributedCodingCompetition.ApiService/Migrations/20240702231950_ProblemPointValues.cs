using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistributedCodingCompetition.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ProblemPointValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemPointValue_Contests_ContestId",
                table: "ProblemPointValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemPointValue",
                table: "ProblemPointValue");

            migrationBuilder.RenameTable(
                name: "ProblemPointValue",
                newName: "ProblemPointValues");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemPointValue_ContestId",
                table: "ProblemPointValues",
                newName: "IX_ProblemPointValues_ContestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemPointValues",
                table: "ProblemPointValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemPointValues_Contests_ContestId",
                table: "ProblemPointValues",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemPointValues_Contests_ContestId",
                table: "ProblemPointValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProblemPointValues",
                table: "ProblemPointValues");

            migrationBuilder.RenameTable(
                name: "ProblemPointValues",
                newName: "ProblemPointValue");

            migrationBuilder.RenameIndex(
                name: "IX_ProblemPointValues_ContestId",
                table: "ProblemPointValue",
                newName: "IX_ProblemPointValue_ContestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProblemPointValue",
                table: "ProblemPointValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemPointValue_Contests_ContestId",
                table: "ProblemPointValue",
                column: "ContestId",
                principalTable: "Contests",
                principalColumn: "Id");
        }
    }
}
