using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAndNutritionApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJoinTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FitnessPlanDetailExercises");

            migrationBuilder.CreateTable(
                name: "FitnessPlanDetailExercise",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(type: "int", nullable: false),
                    FitnessDetailID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessPlanDetailExercise", x => new { x.ExerciseID, x.FitnessDetailID });
                    table.ForeignKey(
                        name: "FK_FitnessPlanDetailExercise_Exercises_ExerciseID",
                        column: x => x.ExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FitnessPlanDetailExercise_FitnessPlanDetails_FitnessDetailID",
                        column: x => x.FitnessDetailID,
                        principalTable: "FitnessPlanDetails",
                        principalColumn: "FitnessDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitnessPlanDetailExercise_FitnessDetailID",
                table: "FitnessPlanDetailExercise",
                column: "FitnessDetailID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FitnessPlanDetailExercise");

            migrationBuilder.CreateTable(
                name: "FitnessPlanDetailExercises",
                columns: table => new
                {
                    ExercisesExerciseID = table.Column<int>(type: "int", nullable: false),
                    FitnessPlanDetailsFitnessDetailID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessPlanDetailExercises", x => new { x.ExercisesExerciseID, x.FitnessPlanDetailsFitnessDetailID });
                    table.ForeignKey(
                        name: "FK_FitnessPlanDetailExercises_Exercises_ExercisesExerciseID",
                        column: x => x.ExercisesExerciseID,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FitnessPlanDetailExercises_FitnessPlanDetails_FitnessPlanDetailsFitnessDetailID",
                        column: x => x.FitnessPlanDetailsFitnessDetailID,
                        principalTable: "FitnessPlanDetails",
                        principalColumn: "FitnessDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FitnessPlanDetailExercises_FitnessPlanDetailsFitnessDetailID",
                table: "FitnessPlanDetailExercises",
                column: "FitnessPlanDetailsFitnessDetailID");
        }
    }
}
