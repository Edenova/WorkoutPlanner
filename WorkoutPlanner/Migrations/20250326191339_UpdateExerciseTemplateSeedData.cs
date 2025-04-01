using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutPlanner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseTemplateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "ExerciseTemplates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Equipment",
                table: "ExerciseTemplates",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MuscleGroup",
                table: "ExerciseTemplates",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ExerciseTemplates",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "ExerciseTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Difficulty", "Equipment", "MuscleGroup", "Type", "VideoUrl" },
                values: new object[] { 2, "Barbell", "Legs", "Legs", "https://www.youtube.com/watch?v=Dh4BtSpExA0" });

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Difficulty", "Equipment", "MuscleGroup", "Type", "VideoUrl" },
                values: new object[] { 1, "Bodyweight", "Chest", "Push", "https://www.youtube.com/watch?v=IODxDxX7oi4" });

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Difficulty", "Equipment", "MuscleGroup", "Type", "VideoUrl" },
                values: new object[] { 3, "Barbell", "Back", "Pull", "https://www.youtube.com/watch?v=ytGaGIn3SjE" });

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Difficulty", "Equipment", "MuscleGroup", "Type", "VideoUrl" },
                values: new object[] { 3, "Barbell", "Chest", "Push", "https://www.youtube.com/watch?v=rT7DgCr-3_E" });

            migrationBuilder.UpdateData(
                table: "ExerciseTemplates",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Difficulty", "Equipment", "MuscleGroup", "Type", "VideoUrl" },
                values: new object[] { 4, "Pull-Up Bar", "Back", "Pull", "https://www.youtube.com/watch?v=eGo4IYlbE5g" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "ExerciseTemplates");

            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "ExerciseTemplates");

            migrationBuilder.DropColumn(
                name: "MuscleGroup",
                table: "ExerciseTemplates");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ExerciseTemplates");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "ExerciseTemplates");
        }
    }
}
