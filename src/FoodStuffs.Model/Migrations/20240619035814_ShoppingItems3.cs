using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class ShoppingItems3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanRecipeRelation",
            table: "MealPlanRecipeRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation");

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
    }
}
