using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class AddMealSet : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Blob_Image",
            table: "Blob");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Category",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Recipe",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_Image_Recipe",
            table: "Image");

        migrationBuilder.DropForeignKey(
            name: "FK_Ingredient_Recipe",
            table: "Ingredient");

        migrationBuilder.DropForeignKey(
            name: "FK_Recipe_PinnedImage",
            table: "Recipe");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AppUser",
            table: "User");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Ingredient",
            table: "Ingredient");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ModifiedOn",
            table: "Ingredient",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedOn",
            table: "Ingredient",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime");

        migrationBuilder.AddPrimaryKey(
            name: "PK_User",
            table: "User",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Ingredient",
            table: "Ingredient",
            columns: new[] { "RecipeId", "Id" });

        migrationBuilder.CreateTable(
            name: "MealSet",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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
            name: "FK_Blob_Image_Id",
            table: "Blob",
            column: "Id",
            principalTable: "Image",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

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

        migrationBuilder.AddForeignKey(
            name: "FK_Image_Recipe_RecipeId",
            table: "Image",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Ingredient_Recipe_RecipeId",
            table: "Ingredient",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Recipe_Image_PinnedImageId",
            table: "Recipe",
            column: "PinnedImageId",
            principalTable: "Image",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Blob_Image_Id",
            table: "Blob");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Category_CategoryId",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipeId",
            table: "CategoryRecipe");

        migrationBuilder.DropForeignKey(
            name: "FK_Image_Recipe_RecipeId",
            table: "Image");

        migrationBuilder.DropForeignKey(
            name: "FK_Ingredient_Recipe_RecipeId",
            table: "Ingredient");

        migrationBuilder.DropForeignKey(
            name: "FK_Recipe_Image_PinnedImageId",
            table: "Recipe");

        migrationBuilder.DropTable(
            name: "MealSetRecipe");

        migrationBuilder.DropTable(
            name: "MealSet");

        migrationBuilder.DropPrimaryKey(
            name: "PK_User",
            table: "User");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Ingredient",
            table: "Ingredient");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ModifiedOn",
            table: "Ingredient",
            type: "datetime",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedOn",
            table: "Ingredient",
            type: "datetime",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");

        migrationBuilder.AddPrimaryKey(
            name: "PK_AppUser",
            table: "User",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Ingredient",
            table: "Ingredient",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Ingredient_RecipeId",
            table: "Ingredient",
            column: "RecipeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Blob_Image",
            table: "Blob",
            column: "Id",
            principalTable: "Image",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Category",
            table: "CategoryRecipe",
            column: "CategoryId",
            principalTable: "Category",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Recipe",
            table: "CategoryRecipe",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Image_Recipe",
            table: "Image",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Ingredient_Recipe",
            table: "Ingredient",
            column: "RecipeId",
            principalTable: "Recipe",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Recipe_PinnedImage",
            table: "Recipe",
            column: "PinnedImageId",
            principalTable: "Image",
            principalColumn: "Id");
    }
}
