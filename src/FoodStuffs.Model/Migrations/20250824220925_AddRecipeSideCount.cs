using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeSideCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealPlanningSidesCount",
                table: "Recipe",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealPlanningSidesCount",
                table: "Recipe");
        }
    }
}
