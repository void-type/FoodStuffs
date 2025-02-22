using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class PantryLocations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PantryLocation",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PantryLocation", x => x.Id);
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
            name: "IX_PantryLocation_Name",
            table: "PantryLocation",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ShoppingItemPantryLocationRelation_ShoppingItemId",
            table: "ShoppingItemPantryLocationRelation",
            column: "ShoppingItemId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ShoppingItemPantryLocationRelation");

        migrationBuilder.DropTable(
            name: "PantryLocation");
    }
}
