using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAndNutritionApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPreparationToMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Preparation",
                table: "Meals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preparation",
                table: "Meals");
        }
    }
}
