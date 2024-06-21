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
            name: "RecipeIngredient");

        migrationBuilder.DropTable(
            name: "RecipeShoppingItem");

        migrationBuilder.DropPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropIndex(
            name: "IX_RecipeShoppingItemRelation_RecipeId",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanRecipeRelation",
            table: "MealPlanRecipeRelation");

        migrationBuilder.DropIndex(
            name: "IX_MealPlanRecipeRelation_MealPlanId",
            table: "MealPlanRecipeRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropIndex(
            name: "IX_MealPlanPantryShoppingItemRelation_MealPlanId",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "MealPlanRecipeRelation");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "ShoppingItem",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<string>(
            name: "Ingredients",
            table: "Recipe",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Category",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation",
            columns: new[] { "RecipeId", "ShoppingItemId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanRecipeRelation",
            table: "MealPlanRecipeRelation",
            columns: new[] { "MealPlanId", "RecipeId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation",
            columns: new[] { "MealPlanId", "ShoppingItemId" });

        migrationBuilder.CreateIndex(
            name: "IX_ShoppingItem_Name",
            table: "ShoppingItem",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Category_Name",
            table: "Category",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ShoppingItem_Name",
            table: "ShoppingItem");

        migrationBuilder.DropPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanRecipeRelation",
            table: "MealPlanRecipeRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropIndex(
            name: "IX_Category_Name",
            table: "Category");

        migrationBuilder.DropColumn(
            name: "Ingredients",
            table: "Recipe");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "ShoppingItem",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "RecipeShoppingItemRelation",
            type: "int",
            nullable: false,
            defaultValue: 0)
            .Annotation("SqlServer:Identity", "1, 1");

        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "MealPlanRecipeRelation",
            type: "int",
            nullable: false,
            defaultValue: 0)
            .Annotation("SqlServer:Identity", "1, 1");

        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "MealPlanPantryShoppingItemRelation",
            type: "int",
            nullable: false,
            defaultValue: 0)
            .Annotation("SqlServer:Identity", "1, 1");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Category",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanRecipeRelation",
            table: "MealPlanRecipeRelation",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "RecipeIngredient",
            columns: table => new
            {
                RecipeId = table.Column<int>(type: "int", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IsCategory = table.Column<bool>(type: "bit", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecipeIngredient", x => new { x.RecipeId, x.Id });
                table.ForeignKey(
                    name: "FK_RecipeIngredient_Recipe_RecipeId",
                    column: x => x.RecipeId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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
            name: "IX_RecipeShoppingItemRelation_RecipeId",
            table: "RecipeShoppingItemRelation",
            column: "RecipeId");

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanRecipeRelation_MealPlanId",
            table: "MealPlanRecipeRelation",
            column: "MealPlanId");

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanPantryShoppingItemRelation_MealPlanId",
            table: "MealPlanPantryShoppingItemRelation",
            column: "MealPlanId");

        migrationBuilder.CreateIndex(
            name: "IX_RecipeShoppingItem_ShoppingItemId",
            table: "RecipeShoppingItem",
            column: "ShoppingItemId");
    }
}
