using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WorkoutPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddRegimens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regimens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regimens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegimenExercises",
                columns: table => new
                {
                    RegimenId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Sets = table.Column<int>(type: "INTEGER", nullable: false),
                    Reps = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegimenExercises", x => new { x.RegimenId, x.ExerciseTemplateId });
                    table.ForeignKey(
                        name: "FK_RegimenExercises_ExerciseTemplates_ExerciseTemplateId",
                        column: x => x.ExerciseTemplateId,
                        principalTable: "ExerciseTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegimenExercises_Regimens_RegimenId",
                        column: x => x.RegimenId,
                        principalTable: "Regimens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Regimens",
                columns: new[] { "Id", "CreatedByUserId", "Description", "Name" },
                values: new object[] { 1, "seed-user", "A simple full-body workout.", "Beginner Full Body" });

            migrationBuilder.InsertData(
                table: "RegimenExercises",
                columns: new[] { "ExerciseTemplateId", "RegimenId", "Reps", "Sets" },
                values: new object[,]
                {
                    { 1, 1, 12, 3 },
                    { 2, 1, 10, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegimenExercises_ExerciseTemplateId",
                table: "RegimenExercises",
                column: "ExerciseTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegimenExercises");

            migrationBuilder.DropTable(
                name: "Regimens");
        }
    }
}
