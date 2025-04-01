using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutPlanner.Migrations
{
    /// <inheritdoc />
    public partial class AddRegimenIdToWorkoutPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_Regimens_RegimenId",
                table: "WorkoutPlans");

            migrationBuilder.AlterColumn<int>(
                name: "RegimenId",
                table: "WorkoutPlans",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_Regimens_RegimenId",
                table: "WorkoutPlans",
                column: "RegimenId",
                principalTable: "Regimens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPlans_Regimens_RegimenId",
                table: "WorkoutPlans");

            migrationBuilder.AlterColumn<int>(
                name: "RegimenId",
                table: "WorkoutPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPlans_Regimens_RegimenId",
                table: "WorkoutPlans",
                column: "RegimenId",
                principalTable: "Regimens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
