using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
#pragma warning disable S125

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Method bodies commented out to migrate pre-Code-First databases. Uncomment to create a new database.
        // migrationBuilder.CreateTable(
        //     name: "Category",
        //     columns: table => new
        //     {
        //         Id = table.Column<int>(type: "int", nullable: false)
        //             .Annotation("SqlServer:Identity", "1, 1"),
        //         Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_Category", x => x.Id);
        //     });

        // migrationBuilder.CreateTable(
        //     name: "User",
        //     columns: table => new
        //     {
        //         Id = table.Column<int>(type: "int", nullable: false)
        //             .Annotation("SqlServer:Identity", "1, 1"),
        //         UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         Password = table.Column<string>(type: "char(128)", unicode: false, fixedLength: true, maxLength: 128, nullable: false),
        //         IsAdmin = table.Column<bool>(type: "bit", nullable: false),
        //         Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_AppUser", x => x.Id);
        //     });

        // migrationBuilder.CreateTable(
        //     name: "Blob",
        //     columns: table => new
        //     {
        //         Id = table.Column<int>(type: "int", nullable: false),
        //         Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_Blob", x => x.Id);
        //     });

        // migrationBuilder.CreateTable(
        //     name: "CategoryRecipe",
        //     columns: table => new
        //     {
        //         RecipeId = table.Column<int>(type: "int", nullable: false),
        //         CategoryId = table.Column<int>(type: "int", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_CategoryRecipe", x => new { x.RecipeId, x.CategoryId });
        //         table.ForeignKey(
        //             name: "FK_CategoryRecipe_Category",
        //             column: x => x.CategoryId,
        //             principalTable: "Category",
        //             principalColumn: "Id",
        //             onDelete: ReferentialAction.Cascade);
        //     });

        // migrationBuilder.CreateTable(
        //     name: "Image",
        //     columns: table => new
        //     {
        //         Id = table.Column<int>(type: "int", nullable: false)
        //             .Annotation("SqlServer:Identity", "1, 1"),
        //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
        //         ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
        //         RecipeId = table.Column<int>(type: "int", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_Image", x => x.Id);
        //     });

        // migrationBuilder.CreateTable(
        //     name: "Recipe",
        //     columns: table => new
        //     {
        //         Id = table.Column<int>(type: "int", nullable: false)
        //             .Annotation("SqlServer:Identity", "1, 1"),
        //         Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         Directions = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         PrepTimeMinutes = table.Column<int>(type: "int", nullable: true),
        //         CookTimeMinutes = table.Column<int>(type: "int", nullable: true),
        //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
        //         ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
        //         PinnedImageId = table.Column<int>(type: "int", nullable: true),
        //         IsForMealPlanning = table.Column<bool>(type: "bit", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_Recipe", x => x.Id);
        //         table.ForeignKey(
        //             name: "FK_Recipe_PinnedImage",
        //             column: x => x.PinnedImageId,
        //             principalTable: "Image",
        //             principalColumn: "Id");
        //     });

        // migrationBuilder.CreateTable(
        //     name: "Ingredient",
        //     columns: table => new
        //     {
        //         Id = table.Column<int>(type: "int", nullable: false)
        //             .Annotation("SqlServer:Identity", "1, 1"),
        //         Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         Quantity = table.Column<int>(type: "int", nullable: false),
        //         Order = table.Column<int>(type: "int", nullable: false),
        //         IsCategory = table.Column<bool>(type: "bit", nullable: false),
        //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
        //         ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
        //         ModifiedOn = table.Column<DateTime>(type: "datetime", nullable: false),
        //         RecipeId = table.Column<int>(type: "int", nullable: false)
        //     },
        //     constraints: table =>
        //     {
        //         table.PrimaryKey("PK_Ingredient", x => x.Id);
        //         table.ForeignKey(
        //             name: "FK_Ingredient_Recipe",
        //             column: x => x.RecipeId,
        //             principalTable: "Recipe",
        //             principalColumn: "Id",
        //             onDelete: ReferentialAction.Cascade);
        //     });

        // migrationBuilder.CreateIndex(
        //     name: "IX_CategoryRecipe_CategoryId",
        //     table: "CategoryRecipe",
        //     column: "CategoryId");

        // migrationBuilder.CreateIndex(
        //     name: "IX_Image_RecipeId",
        //     table: "Image",
        //     column: "RecipeId");

        // migrationBuilder.CreateIndex(
        //     name: "IX_Ingredient_RecipeId",
        //     table: "Ingredient",
        //     column: "RecipeId");

        // migrationBuilder.CreateIndex(
        //     name: "IX_Recipe_PinnedImageId",
        //     table: "Recipe",
        //     column: "PinnedImageId");

        // migrationBuilder.AddForeignKey(
        //     name: "FK_Blob_Image",
        //     table: "Blob",
        //     column: "Id",
        //     principalTable: "Image",
        //     principalColumn: "Id",
        //     onDelete: ReferentialAction.Cascade);

        // migrationBuilder.AddForeignKey(
        //     name: "FK_CategoryRecipe_Recipe",
        //     table: "CategoryRecipe",
        //     column: "RecipeId",
        //     principalTable: "Recipe",
        //     principalColumn: "Id",
        //     onDelete: ReferentialAction.Cascade);

        // migrationBuilder.AddForeignKey(
        //     name: "FK_Image_Recipe",
        //     table: "Image",
        //     column: "RecipeId",
        //     principalTable: "Recipe",
        //     principalColumn: "Id",
        //     onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Method bodies commented out to migrate pre-Code-First databases. Uncomment to create a new database.
        // migrationBuilder.DropForeignKey(
        //     name: "FK_Recipe_PinnedImage",
        //     table: "Recipe");

        // migrationBuilder.DropTable(
        //     name: "Blob");

        // migrationBuilder.DropTable(
        //     name: "CategoryRecipe");

        // migrationBuilder.DropTable(
        //     name: "Ingredient");

        // migrationBuilder.DropTable(
        //     name: "User");

        // migrationBuilder.DropTable(
        //     name: "Category");

        // migrationBuilder.DropTable(
        //     name: "Image");

        // migrationBuilder.DropTable(
        //     name: "Recipe");
    }
}
