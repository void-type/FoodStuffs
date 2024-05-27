using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class AddMealPlans : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MealPlan",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                PantryIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlan", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MealPlanRecipe",
            columns: table => new
            {
                MealPlansId = table.Column<int>(type: "int", nullable: false),
                RecipesId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlanRecipe", x => new { x.MealPlansId, x.RecipesId });
                table.ForeignKey(
                    name: "FK_MealPlanRecipe_MealPlan_MealPlansId",
                    column: x => x.MealPlansId,
                    principalTable: "MealPlan",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MealPlanRecipe_Recipe_RecipesId",
                    column: x => x.RecipesId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanRecipe_RecipesId",
            table: "MealPlanRecipe",
            column: "RecipesId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MealPlanRecipe");

        migrationBuilder.DropTable(
            name: "MealPlan");
    }
}
