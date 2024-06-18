using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class ShoppingItems2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RecipeShoppingItem");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RecipeShoppingItem",
            columns: table => new
            {
                RecipesId = table.Column<int>(type: "int", nullable: false),
                ShoppingItemId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecipeShoppingItem", x => new { x.RecipesId, x.ShoppingItemId });
                table.ForeignKey(
                    name: "FK_RecipeShoppingItem_Recipe_RecipesId",
                    column: x => x.RecipesId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RecipeShoppingItem_ShoppingItem_ShoppingItemId",
                    column: x => x.ShoppingItemId,
                    principalTable: "ShoppingItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RecipeShoppingItem_ShoppingItemId",
            table: "RecipeShoppingItem",
            column: "ShoppingItemId");
    }
}
