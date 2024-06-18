using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class ShoppingItems : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Category_CategoriesId",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipesId",
            table: "CategoryRecipe");

        migrationBuilder.DropTable(
            name: "MealSetRecipe");

        migrationBuilder.DropTable(
            name: "MealSet");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "RecipeIngredient");

        migrationBuilder.DropColumn(
            name: "CreatedOn",
            table: "RecipeIngredient");

        migrationBuilder.DropColumn(
            name: "ModifiedBy",
            table: "RecipeIngredient");

        migrationBuilder.DropColumn(
            name: "ModifiedOn",
            table: "RecipeIngredient");

        migrationBuilder.RenameColumn(
            name: "RecipesId",
            table: "CategoryRecipe",
            newName: "RecipeId");

        migrationBuilder.RenameColumn(
            name: "CategoriesId",
            table: "CategoryRecipe",
            newName: "CategoryId");

        migrationBuilder.RenameIndex(
            name: "IX_CategoryRecipe_RecipesId",
            table: "CategoryRecipe",
            newName: "IX_CategoryRecipe_RecipeId");

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
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlan", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ShoppingItem",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShoppingItem", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MealPlanRecipeRelation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Order = table.Column<int>(type: "int", nullable: false),
                MealPlanId = table.Column<int>(type: "int", nullable: false),
                RecipeId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlanRecipeRelation", x => x.Id);
                table.ForeignKey(
                    name: "FK_MealPlanRecipeRelation_MealPlan_MealPlanId",
                    column: x => x.MealPlanId,
                    principalTable: "MealPlan",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MealPlanRecipeRelation_Recipe_RecipeId",
                    column: x => x.RecipeId,
                    principalTable: "Recipe",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MealPlanPantryShoppingItemRelation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Quantity = table.Column<int>(type: "int", nullable: false),
                MealPlanId = table.Column<int>(type: "int", nullable: false),
                ShoppingItemId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealPlanPantryShoppingItemRelation", x => x.Id);
                table.ForeignKey(
                    name: "FK_MealPlanPantryShoppingItemRelation_MealPlan_MealPlanId",
                    column: x => x.MealPlanId,
                    principalTable: "MealPlan",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MealPlanPantryShoppingItemRelation_ShoppingItem_ShoppingItemId",
                    column: x => x.ShoppingItemId,
                    principalTable: "ShoppingItem",
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

        migrationBuilder.CreateTable(
            name: "RecipeShoppingItemRelation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Quantity = table.Column<int>(type: "int", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                RecipeId = table.Column<int>(type: "int", nullable: false),
                ShoppingItemId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecipeShoppingItemRelation", x => x.Id);
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

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanPantryShoppingItemRelation_MealPlanId",
            table: "MealPlanPantryShoppingItemRelation",
            column: "MealPlanId");

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanPantryShoppingItemRelation_ShoppingItemId",
            table: "MealPlanPantryShoppingItemRelation",
            column: "ShoppingItemId");

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanRecipeRelation_MealPlanId",
            table: "MealPlanRecipeRelation",
            column: "MealPlanId");

        migrationBuilder.CreateIndex(
            name: "IX_MealPlanRecipeRelation_RecipeId",
            table: "MealPlanRecipeRelation",
            column: "RecipeId");

        migrationBuilder.CreateIndex(
            name: "IX_RecipeShoppingItem_ShoppingItemId",
            table: "RecipeShoppingItem",
            column: "ShoppingItemId");

        migrationBuilder.CreateIndex(
            name: "IX_RecipeShoppingItemRelation_RecipeId",
            table: "RecipeShoppingItemRelation",
            column: "RecipeId");

        migrationBuilder.CreateIndex(
            name: "IX_RecipeShoppingItemRelation_ShoppingItemId",
            table: "RecipeShoppingItemRelation",
            column: "ShoppingItemId");

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Category_CategoryId",
            table: "CategoryRecipe",
            column: "CategoryId",
            principalTable: "Category",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipeId",
            table: "CategoryRecipe",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Category_CategoryId",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipeId",
            table: "CategoryRecipe");

        migrationBuilder.DropTable(
            name: "MealPlanPantryShoppingItemRelation");

        migrationBuilder.DropTable(
            name: "MealPlanRecipeRelation");

        migrationBuilder.DropTable(
            name: "RecipeShoppingItem");

        migrationBuilder.DropTable(
            name: "RecipeShoppingItemRelation");

        migrationBuilder.DropTable(
            name: "MealPlan");

        migrationBuilder.DropTable(
            name: "ShoppingItem");

        migrationBuilder.RenameColumn(
            name: "RecipeId",
            table: "CategoryRecipe",
            newName: "RecipesId");

        migrationBuilder.RenameColumn(
            name: "CategoryId",
            table: "CategoryRecipe",
            newName: "CategoriesId");

        migrationBuilder.RenameIndex(
            name: "IX_CategoryRecipe_RecipeId",
            table: "CategoryRecipe",
            newName: "IX_CategoryRecipe_RecipesId");

        migrationBuilder.AddColumn<string>(
            name: "CreatedBy",
            table: "RecipeIngredient",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "CreatedOn",
            table: "RecipeIngredient",
            type: "datetimeoffset",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

        migrationBuilder.AddColumn<string>(
            name: "ModifiedBy",
            table: "RecipeIngredient",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "ModifiedOn",
            table: "RecipeIngredient",
            type: "datetimeoffset",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

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

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Category_CategoriesId",
            table: "CategoryRecipe",
            column: "CategoriesId",
            principalTable: "Category",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipesId",
            table: "CategoryRecipe",
            column: "RecipesId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
