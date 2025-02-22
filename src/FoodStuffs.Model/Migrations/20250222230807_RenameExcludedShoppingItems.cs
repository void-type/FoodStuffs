using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RenameExcludedShoppingItems : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "MealPlanPantryShoppingItemRelation",
            newName: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanPantryShoppingItemRelation_MealPlan_MealPlanId",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanPantryShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropIndex(
            name: "IX_MealPlanPantryShoppingItemRelation_ShoppingItemId",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanExcludedShoppingItemRelation",
            table: "MealPlanExcludedShoppingItemRelation",
            columns: ["MealPlanId", "ShoppingItemId"]);

        migrationBuilder.AddForeignKey(
            name: "FK_MealPlanExcludedShoppingItemRelation_MealPlan_MealPlanId",
            table: "MealPlanExcludedShoppingItemRelation",
            column: "MealPlanId",
            principalTable: "MealPlan",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MealPlanExcludedShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "MealPlanExcludedShoppingItemRelation",
            column: "ShoppingItemId",
            principalTable: "ShoppingItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanExcludedShoppingItemRelation_ShoppingItemId",
            table: "MealPlanExcludedShoppingItemRelation",
            column: "ShoppingItemId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameTable(
            name: "MealPlanExcludedShoppingItemRelation",
            newName: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanExcludedShoppingItemRelation_MealPlan_MealPlanId",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanExcludedShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanExcludedShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropIndex(
            name: "IX_MealPlanExcludedShoppingItemRelation_ShoppingItemId",
            table: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanPantryShoppingItemRelation",
            table: "MealPlanPantryShoppingItemRelation",
            columns: ["MealPlanId", "ShoppingItemId"]);

        migrationBuilder.AddForeignKey(
            name: "FK_MealPlanPantryShoppingItemRelation_MealPlan_MealPlanId",
            table: "MealPlanPantryShoppingItemRelation",
            column: "MealPlanId",
            principalTable: "MealPlan",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MealPlanPantryShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "MealPlanPantryShoppingItemRelation",
            column: "ShoppingItemId",
            principalTable: "ShoppingItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanPantryShoppingItemRelation_ShoppingItemId",
            table: "MealPlanPantryShoppingItemRelation",
            column: "ShoppingItemId");
    }
}
