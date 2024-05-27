using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RemoveMealSets : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MealSetRecipe");

        migrationBuilder.DropTable(
            name: "MealSet");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "MealSet",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PantryIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealSet", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MealSetRecipe",
            columns: table => new
            {
                MealSetsId = table.Column<int>(type: "int", nullable: false),
                RecipesId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealSetRecipe", x => new { x.MealSetsId, x.RecipesId });
                table.ForeignKey(
                    name: "FK_MealSetRecipe_MealSet_MealSetsId",
                    column: x => x.MealSetsId,
                    principalTable: "MealSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MealSetRecipe_Recipe_RecipesId",
                    column: x => x.RecipesId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MealSetRecipe_RecipesId",
            table: "MealSetRecipe",
            column: "RecipesId");
    }
}
