using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessAndNutritionApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDayOfWeekToExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Exercises");
        }
    }
}
