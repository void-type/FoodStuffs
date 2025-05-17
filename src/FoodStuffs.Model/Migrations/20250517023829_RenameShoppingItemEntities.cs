using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RenameShoppingItemEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MealPlanExcludedShoppingItemRelation");

        migrationBuilder.DropTable(
            name: "RecipeShoppingItemRelation");

        migrationBuilder.DropTable(
            name: "ShoppingItemPantryLocationRelation");

        migrationBuilder.DropTable(
            name: "ShoppingItem");

        migrationBuilder.CreateTable(
            name: "GroceryItem",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                InventoryQuantity = table.Column<int>(type: "int", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                GroceryDepartmentId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GroceryItem", x => x.Id);
                table.ForeignKey(
                    name: "FK_GroceryItem_GroceryDepartment_GroceryDepartmentId",
                    column: x => x.GroceryDepartmentId,
                    principalTable: "GroceryDepartment",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "GroceryItemPantryLocationRelation",
            columns: table => new
            {
                GroceryItemId = table.Column<int>(type: "int", nullable: false),
                PantryLocationId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GroceryItemPantryLocationRelation", x => new { x.GroceryItemId, x.PantryLocationId });
                table.ForeignKey(
                    name: "FK_GroceryItemPantryLocationRelation_GroceryItem_GroceryItemId",
                    column: x => x.GroceryItemId,
                    principalTable: "GroceryItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GroceryItemPantryLocationRelation_PantryLocation_PantryLocationId",
                    column: x => x.PantryLocationId,
                    principalTable: "PantryLocation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MealPlanExcludedGroceryItemRelation",
            columns: table => new
            {
                MealPlanId = table.Column<int>(type: "int", nullable: false),
                GroceryItemId = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlanExcludedGroceryItemRelation", x => new { x.MealPlanId, x.GroceryItemId });
                table.ForeignKey(
                    name: "FK_MealPlanExcludedGroceryItemRelation_GroceryItem_GroceryItemId",
                    column: x => x.GroceryItemId,
                    principalTable: "GroceryItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MealPlanExcludedGroceryItemRelation_MealPlan_MealPlanId",
                    column: x => x.MealPlanId,
                    principalTable: "MealPlan",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RecipeGroceryItemRelation",
            columns: table => new
            {
                RecipeId = table.Column<int>(type: "int", nullable: false),
                GroceryItemId = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecipeGroceryItemRelation", x => new { x.RecipeId, x.GroceryItemId });
                table.ForeignKey(
                    name: "FK_RecipeGroceryItemRelation_GroceryItem_GroceryItemId",
                    column: x => x.GroceryItemId,
                    principalTable: "GroceryItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RecipeGroceryItemRelation_Recipe_RecipeId",
                    column: x => x.RecipeId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GroceryItem_GroceryDepartmentId",
            table: "GroceryItem",
            column: "GroceryDepartmentId");

        migrationBuilder.CreateIndex(
            name: "IX_GroceryItem_Name",
            table: "GroceryItem",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GroceryItemPantryLocationRelation_PantryLocationId",
            table: "GroceryItemPantryLocationRelation",
            column: "PantryLocationId");

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanExcludedGroceryItemRelation_GroceryItemId",
            table: "MealPlanExcludedGroceryItemRelation",
            column: "GroceryItemId");

        migrationBuilder.CreateIndex(
            name: "IX_RecipeGroceryItemRelation_GroceryItemId",
            table: "RecipeGroceryItemRelation",
            column: "GroceryItemId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropTable(
            name: "MealPlanExcludedGroceryItemRelation");

        migrationBuilder.DropTable(
            name: "RecipeGroceryItemRelation");

        migrationBuilder.DropTable(
            name: "GroceryItem");

        migrationBuilder.CreateTable(
            name: "ShoppingItem",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GroceryDepartmentId = table.Column<int>(type: "int", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                InventoryQuantity = table.Column<int>(type: "int", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShoppingItem", x => x.Id);
                table.ForeignKey(
                    name: "FK_ShoppingItem_GroceryDepartment_GroceryDepartmentId",
                    column: x => x.GroceryDepartmentId,
                    principalTable: "GroceryDepartment",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "MealPlanExcludedShoppingItemRelation",
            columns: table => new
            {
                MealPlanId = table.Column<int>(type: "int", nullable: false),
                ShoppingItemId = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlanExcludedShoppingItemRelation", x => new { x.MealPlanId, x.ShoppingItemId });
                table.ForeignKey(
                    name: "FK_MealPlanExcludedShoppingItemRelation_MealPlan_MealPlanId",
                    column: x => x.MealPlanId,
                    principalTable: "MealPlan",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MealPlanExcludedShoppingItemRelation_ShoppingItem_ShoppingItemId",
                    column: x => x.ShoppingItemId,
                    principalTable: "ShoppingItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RecipeShoppingItemRelation",
            columns: table => new
            {
                RecipeId = table.Column<int>(type: "int", nullable: false),
                ShoppingItemId = table.Column<int>(type: "int", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecipeShoppingItemRelation", x => new { x.RecipeId, x.ShoppingItemId });
                table.ForeignKey(
                    name: "FK_RecipeShoppingItemRelation_Recipe_RecipeId",
                    column: x => x.RecipeId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RecipeShoppingItemRelation_ShoppingItem_ShoppingItemId",
                    column: x => x.ShoppingItemId,
                    principalTable: "ShoppingItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ShoppingItemPantryLocationRelation",
            columns: table => new
            {
                PantryLocationId = table.Column<int>(type: "int", nullable: false),
                ShoppingItemId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShoppingItemPantryLocationRelation", x => new { x.PantryLocationId, x.ShoppingItemId });
                table.ForeignKey(
                    name: "FK_ShoppingItemPantryLocationRelation_PantryLocation_PantryLocationId",
                    column: x => x.PantryLocationId,
                    principalTable: "PantryLocation",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ShoppingItemPantryLocationRelation_ShoppingItem_ShoppingItemId",
                    column: x => x.ShoppingItemId,
                    principalTable: "ShoppingItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanExcludedShoppingItemRelation_ShoppingItemId",
            table: "MealPlanExcludedShoppingItemRelation",
            column: "ShoppingItemId");

        migrationBuilder.CreateIndex(
            name: "IX_RecipeShoppingItemRelation_ShoppingItemId",
            table: "RecipeShoppingItemRelation",
            column: "ShoppingItemId");

        migrationBuilder.CreateIndex(
            name: "IX_ShoppingItem_GroceryDepartmentId",
            table: "ShoppingItem",
            column: "GroceryDepartmentId");

        migrationBuilder.CreateIndex(
            name: "IX_ShoppingItem_Name",
            table: "ShoppingItem",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ShoppingItemPantryLocationRelation_ShoppingItemId",
            table: "ShoppingItemPantryLocationRelation",
            column: "ShoppingItemId");
    }
}
