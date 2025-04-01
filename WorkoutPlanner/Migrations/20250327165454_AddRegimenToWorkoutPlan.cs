using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddRegimenToWorkoutPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegimenId",
                table: "WorkoutPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_RegimenId",
                table: "WorkoutPlans",
                column: "RegimenId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_Regimens_RegimenId",
                table: "WorkoutPlans",
                column: "RegimenId",
                principalTable: "Regimens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_Regimens_RegimenId",
                table: "WorkoutPlans");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPlans_RegimenId",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "RegimenId",
                table: "WorkoutPlans");
        }
    }
}
