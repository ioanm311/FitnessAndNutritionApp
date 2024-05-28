using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAndNutritionApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNutritionLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.MealID);
                });

            migrationBuilder.CreateTable(
                name: "NutritionPlanDetails",
                columns: table => new
                {
                    NutritionDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutritionPlanID = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionPlanDetails", x => x.NutritionDetailID);
                    table.ForeignKey(
                        name: "FK_NutritionPlanDetails_NutritionPlans_NutritionPlanID",
                        column: x => x.NutritionPlanID,
                        principalTable: "NutritionPlans",
                        principalColumn: "NutritionPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NutritionPlanDetailMeal",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false),
                    NutritionDetailID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionPlanDetailMeal", x => new { x.MealID, x.NutritionDetailID });
                    table.ForeignKey(
                        name: "FK_NutritionPlanDetailMeal_Meals_MealID",
                        column: x => x.MealID,
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NutritionPlanDetailMeal_NutritionPlanDetails_NutritionDetailID",
                        column: x => x.NutritionDetailID,
                        principalTable: "NutritionPlanDetails",
                        principalColumn: "NutritionDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlanDetailMeal_NutritionDetailID",
                table: "NutritionPlanDetailMeal",
                column: "NutritionDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPlanDetails_NutritionPlanID",
                table: "NutritionPlanDetails",
                column: "NutritionPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutritionPlanDetailMeal");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "NutritionPlanDetails");
        }
    }
}
