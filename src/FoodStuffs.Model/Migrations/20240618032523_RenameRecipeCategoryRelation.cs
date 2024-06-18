using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RenameRecipeCategoryRelation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_CategoryRecipe",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Category_CategoryId",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipeId",
            table: "CategoryRecipe");

        migrationBuilder.RenameTable(
            name: "CategoryRecipe",
            newName: "RecipeCategoryRelation");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RecipeCategoryRelation",
            table: "RecipeCategoryRelation",
            columns: ["CategoryId", "RecipeId"]);

        migrationBuilder.AddForeignKey(
            name: "FK_RecipeCategoryRelation_Category_CategoryId",
            table: "RecipeCategoryRelation",
            column: "CategoryId",
            principalTable: "Category",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_RecipeCategoryRelation_Recipe_RecipeId",
            table: "RecipeCategoryRelation",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.RenameIndex(
            name: "IX_CategoryRecipe_RecipeId",
            newName: "IX_RecipeCategoryRelation_RecipeId",
            table: "RecipeCategoryRelation");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_RecipeCategoryRelation",
            table: "RecipeCategoryRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_RecipeCategoryRelation_Category_CategoryId",
            table: "RecipeCategoryRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_RecipeCategoryRelation_Recipe_RecipeId",
            table: "RecipeCategoryRelation");

        migrationBuilder.RenameTable(
            name: "RecipeCategoryRelation",
            newName: "CategoryRecipe");

        migrationBuilder.AddPrimaryKey(
            name: "PK_CategoryRecipe",
            table: "CategoryRecipe",
            columns: ["CategoryId", "RecipeId"]);

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

        migrationBuilder.RenameIndex(
            name: "IX_RecipeCategoryRelation_RecipeId",
            newName: "IX_CategoryRecipe_RecipeId",
            table: "CategoryRecipe");
    }
}
