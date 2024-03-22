using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class Reinitialize : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Category",
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
                table.PrimaryKey("PK_Category", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MealSet",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                PantryIngredients = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MealSet", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CategoryRecipe",
            columns: table => new
            {
                CategoriesId = table.Column<int>(type: "int", nullable: false),
                RecipesId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CategoryRecipe", x => new { x.CategoriesId, x.RecipesId });
                table.ForeignKey(
                    name: "FK_CategoryRecipe_Category_CategoriesId",
                    column: x => x.CategoriesId,
                    principalTable: "Category",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Image",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FileName = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                RecipeId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Image", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ImageBlob",
            columns: table => new
            {
                ImageId = table.Column<int>(type: "int", nullable: false),
                Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ImageBlob", x => x.ImageId);
                table.ForeignKey(
                    name: "FK_ImageBlob_Image_ImageId",
                    column: x => x.ImageId,
                    principalTable: "Image",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Recipe",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Directions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PrepTimeMinutes = table.Column<int>(type: "int", nullable: true),
                CookTimeMinutes = table.Column<int>(type: "int", nullable: true),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                PinnedImageId = table.Column<int>(type: "int", nullable: true),
                IsForMealPlanning = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Recipe", x => x.Id);
                table.ForeignKey(
                    name: "FK_Recipe_Image_PinnedImageId",
                    column: x => x.PinnedImageId,
                    principalTable: "Image",
                    principalColumn: "Id");
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

        migrationBuilder.CreateTable(
            name: "RecipeIngredient",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RecipeId = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                IsCategory = table.Column<bool>(type: "bit", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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

        migrationBuilder.CreateIndex(
            name: "IX_CategoryRecipe_RecipesId",
            table: "CategoryRecipe",
            column: "RecipesId");

        migrationBuilder.CreateIndex(
            name: "IX_Image_FileName",
            table: "Image",
            column: "FileName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Image_RecipeId",
            table: "Image",
            column: "RecipeId");

        migrationBuilder.CreateIndex(
            name: "IX_MealSetRecipe_RecipesId",
            table: "MealSetRecipe",
            column: "RecipesId");

        migrationBuilder.CreateIndex(
            name: "IX_Recipe_PinnedImageId",
            table: "Recipe",
            column: "PinnedImageId");

        migrationBuilder.AddForeignKey(
            name: "FK_CategoryRecipe_Recipe_RecipesId",
            table: "CategoryRecipe",
            column: "RecipesId",
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
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Image_Recipe_RecipeId",
            table: "Image");

        migrationBuilder.DropTable(
            name: "CategoryRecipe");

        migrationBuilder.DropTable(
            name: "ImageBlob");

        migrationBuilder.DropTable(
            name: "MealSetRecipe");

        migrationBuilder.DropTable(
            name: "RecipeIngredient");

        migrationBuilder.DropTable(
            name: "Category");

        migrationBuilder.DropTable(
            name: "MealSet");

        migrationBuilder.DropTable(
            name: "Recipe");

        migrationBuilder.DropTable(
            name: "Image");
    }
}
