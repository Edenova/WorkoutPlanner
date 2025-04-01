using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToRegimenExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RegimenExercises",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "RegimenExercises",
                keyColumns: new[] { "ExerciseTemplateId", "RegimenId" },
                keyValues: new object[] { 1, 1 },
                column: "Id",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RegimenExercises",
                keyColumns: new[] { "ExerciseTemplateId", "RegimenId" },
                keyValues: new object[] { 2, 1 },
                column: "Id",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "RegimenExercises");
        }
    }
}
