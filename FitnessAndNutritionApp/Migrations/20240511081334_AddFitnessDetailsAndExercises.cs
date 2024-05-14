using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAndNutritionApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFitnessDetailsAndExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseID);
                });

            migrationBuilder.CreateTable(
                name: "FitnessPlanDetails",
                columns: table => new
                {
                    FitnessDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FitnessPlanID = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessPlanDetails", x => x.FitnessDetailID);
                    table.ForeignKey(
                        name: "FK_FitnessPlanDetails_FitnessPlans_FitnessPlanID",
                        column: x => x.FitnessPlanID,
                        principalTable: "FitnessPlans",
                        principalColumn: "FitnessPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_FitnessPlanDetails_FitnessPlanID",
                table: "FitnessPlanDetails",
                column: "FitnessPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FitnessPlanDetailExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "FitnessPlanDetails");
        }
    }
}
