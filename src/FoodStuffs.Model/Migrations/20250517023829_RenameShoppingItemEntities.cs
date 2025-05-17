using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RenameShoppingItemEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ShoppingItem_GroceryDepartment_GroceryDepartmentId",
            table: "ShoppingItem");

        migrationBuilder.DropForeignKey(
            name: "FK_ShoppingItemPantryLocationRelation_PantryLocation_PantryLocationId",
            table: "ShoppingItemPantryLocationRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_ShoppingItemPantryLocationRelation_ShoppingItem_ShoppingItemId",
            table: "ShoppingItemPantryLocationRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanExcludedShoppingItemRelation_MealPlan_MealPlanId",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanExcludedShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_RecipeShoppingItemRelation_Recipe_RecipeId",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_RecipeShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ShoppingItemPantryLocationRelation",
            table: "ShoppingItemPantryLocationRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanExcludedShoppingItemRelation",
            table: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ShoppingItem",
            table: "ShoppingItem");

        migrationBuilder.RenameTable(
            name: "ShoppingItem",
            newName: "GroceryItem");

        migrationBuilder.RenameTable(
            name: "ShoppingItemPantryLocationRelation",
            newName: "GroceryItemPantryLocationRelation");

        migrationBuilder.RenameTable(
            name: "MealPlanExcludedShoppingItemRelation",
            newName: "MealPlanExcludedGroceryItemRelation");

        migrationBuilder.RenameTable(
            name: "RecipeShoppingItemRelation",
            newName: "RecipeGroceryItemRelation");

        migrationBuilder.RenameColumn(
            name: "ShoppingItemId",
            table: "GroceryItemPantryLocationRelation",
            newName: "GroceryItemId");

        migrationBuilder.RenameColumn(
            name: "ShoppingItemId",
            table: "MealPlanExcludedGroceryItemRelation",
            newName: "GroceryItemId");

        migrationBuilder.RenameColumn(
            name: "ShoppingItemId",
            table: "RecipeGroceryItemRelation",
            newName: "GroceryItemId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GroceryItem",
            table: "GroceryItem",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GroceryItemPantryLocationRelation",
            table: "GroceryItemPantryLocationRelation",
            columns: new[] { "GroceryItemId", "PantryLocationId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanExcludedGroceryItemRelation",
            table: "MealPlanExcludedGroceryItemRelation",
            columns: new[] { "MealPlanId", "GroceryItemId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_RecipeGroceryItemRelation",
            table: "RecipeGroceryItemRelation",
            columns: new[] { "RecipeId", "GroceryItemId" });

        migrationBuilder.RenameIndex(
            name: "IX_ShoppingItem_Name",
            table: "GroceryItem",
            newName: "IX_GroceryItem_Name");

        migrationBuilder.RenameIndex(
            name: "IX_ShoppingItem_GroceryDepartmentId",
            table: "GroceryItem",
            newName: "IX_GroceryItem_GroceryDepartmentId");

        migrationBuilder.RenameIndex(
            name: "IX_ShoppingItemPantryLocationRelation_ShoppingItemId",
            table: "GroceryItemPantryLocationRelation",
            newName: "IX_GroceryItemPantryLocationRelation_PantryLocationId");

        migrationBuilder.RenameIndex(
            name: "IX_MealPlanExcludedShoppingItemRelation_ShoppingItemId",
            table: "MealPlanExcludedGroceryItemRelation",
            newName: "IX_MealPlanExcludedGroceryItemRelation_GroceryItemId");

        migrationBuilder.RenameIndex(
            name: "IX_RecipeShoppingItemRelation_ShoppingItemId",
            table: "RecipeGroceryItemRelation",
            newName: "IX_RecipeGroceryItemRelation_GroceryItemId");

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItem_GroceryDepartment_GroceryDepartmentId",
            table: "GroceryItem",
            column: "GroceryDepartmentId",
            principalTable: "GroceryDepartment",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItemPantryLocationRelation_GroceryItem_GroceryItemId",
            table: "GroceryItemPantryLocationRelation",
            column: "GroceryItemId",
            principalTable: "GroceryItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItemPantryLocationRelation_PantryLocation_PantryLocationId",
            table: "GroceryItemPantryLocationRelation",
            column: "PantryLocationId",
            principalTable: "PantryLocation",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MealPlanExcludedGroceryItemRelation_GroceryItem_GroceryItemId",
            table: "MealPlanExcludedGroceryItemRelation",
            column: "GroceryItemId",
            principalTable: "GroceryItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_MealPlanExcludedGroceryItemRelation_MealPlan_MealPlanId",
            table: "MealPlanExcludedGroceryItemRelation",
            column: "MealPlanId",
            principalTable: "MealPlan",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_RecipeGroceryItemRelation_GroceryItem_GroceryItemId",
            table: "RecipeGroceryItemRelation",
            column: "GroceryItemId",
            principalTable: "GroceryItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_RecipeGroceryItemRelation_Recipe_RecipeId",
            table: "RecipeGroceryItemRelation",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItem_GroceryDepartment_GroceryDepartmentId",
            table: "GroceryItem");

        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItemPantryLocationRelation_GroceryItem_GroceryItemId",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItemPantryLocationRelation_PantryLocation_PantryLocationId",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanExcludedGroceryItemRelation_GroceryItem_GroceryItemId",
            table: "MealPlanExcludedGroceryItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_MealPlanExcludedGroceryItemRelation_MealPlan_MealPlanId",
            table: "MealPlanExcludedGroceryItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_RecipeGroceryItemRelation_GroceryItem_GroceryItemId",
            table: "RecipeGroceryItemRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_RecipeGroceryItemRelation_Recipe_RecipeId",
            table: "RecipeGroceryItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GroceryItem",
            table: "GroceryItem");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GroceryItemPantryLocationRelation",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_MealPlanExcludedGroceryItemRelation",
            table: "MealPlanExcludedGroceryItemRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_RecipeGroceryItemRelation",
            table: "RecipeGroceryItemRelation");

        migrationBuilder.RenameColumn(
            name: "GroceryItemId",
            table: "RecipeGroceryItemRelation",
            newName: "ShoppingItemId");

        migrationBuilder.RenameColumn(
            name: "GroceryItemId",
            table: "MealPlanExcludedGroceryItemRelation",
            newName: "ShoppingItemId");

        migrationBuilder.RenameColumn(
            name: "GroceryItemId",
            table: "GroceryItemPantryLocationRelation",
            newName: "ShoppingItemId");

        migrationBuilder.RenameTable(
            name: "RecipeGroceryItemRelation",
            newName: "RecipeShoppingItemRelation");

        migrationBuilder.RenameTable(
            name: "MealPlanExcludedGroceryItemRelation",
            newName: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.RenameTable(
            name: "GroceryItemPantryLocationRelation",
            newName: "ShoppingItemPantryLocationRelation");

        migrationBuilder.RenameTable(
            name: "GroceryItem",
            newName: "ShoppingItem");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ShoppingItem",
            table: "ShoppingItem",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ShoppingItemPantryLocationRelation",
            table: "ShoppingItemPantryLocationRelation",
            columns: new[] { "PantryLocationId", "ShoppingItemId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_MealPlanExcludedShoppingItemRelation",
            table: "MealPlanExcludedShoppingItemRelation",
            columns: new[] { "MealPlanId", "ShoppingItemId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_RecipeShoppingItemRelation",
            table: "RecipeShoppingItemRelation",
            columns: new[] { "RecipeId", "ShoppingItemId" });

        migrationBuilder.RenameIndex(
            name: "IX_RecipeGroceryItemRelation_GroceryItemId",
            table: "RecipeShoppingItemRelation",
            newName: "IX_RecipeShoppingItemRelation_ShoppingItemId");

        migrationBuilder.RenameIndex(
            name: "IX_MealPlanExcludedGroceryItemRelation_GroceryItemId",
            table: "MealPlanExcludedShoppingItemRelation",
            newName: "IX_MealPlanExcludedShoppingItemRelation_ShoppingItemId");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItemPantryLocationRelation_PantryLocationId",
            table: "ShoppingItemPantryLocationRelation",
            newName: "IX_ShoppingItemPantryLocationRelation_ShoppingItemId");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItem_Name",
            table: "ShoppingItem",
            newName: "IX_ShoppingItem_Name");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItem_GroceryDepartmentId",
            table: "ShoppingItem",
            newName: "IX_ShoppingItem_GroceryDepartmentId");

        migrationBuilder.AddForeignKey(
            name: "FK_ShoppingItem_GroceryDepartment_GroceryDepartmentId",
            table: "ShoppingItem",
            column: "GroceryDepartmentId",
            principalTable: "GroceryDepartment",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ShoppingItemPantryLocationRelation_PantryLocation_PantryLocationId",
            table: "ShoppingItemPantryLocationRelation",
            column: "PantryLocationId",
            principalTable: "PantryLocation",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ShoppingItemPantryLocationRelation_ShoppingItem_ShoppingItemId",
            table: "ShoppingItemPantryLocationRelation",
            column: "ShoppingItemId",
            principalTable: "ShoppingItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

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

        migrationBuilder.AddForeignKey(
            name: "FK_RecipeShoppingItemRelation_Recipe_RecipeId",
            table: "RecipeShoppingItemRelation",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_RecipeShoppingItemRelation_ShoppingItem_ShoppingItemId",
            table: "RecipeShoppingItemRelation",
            column: "ShoppingItemId",
            principalTable: "ShoppingItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
