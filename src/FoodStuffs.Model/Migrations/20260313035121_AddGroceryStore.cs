using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class AddGroceryStore : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "GroceryStore",
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
                table.PrimaryKey("PK_GroceryStore", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GroceryItemGroceryStoreRelation",
            columns: table => new
            {
                GroceryItemId = table.Column<int>(type: "int", nullable: false),
                GroceryStoreId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GroceryItemGroceryStoreRelation", x => new { x.GroceryItemId, x.GroceryStoreId });
                table.ForeignKey(
                    name: "FK_GroceryItemGroceryStoreRelation_GroceryItem_GroceryItemId",
                    column: x => x.GroceryItemId,
                    principalTable: "GroceryItem",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GroceryItemGroceryStoreRelation_GroceryStore_GroceryStoreId",
                    column: x => x.GroceryStoreId,
                    principalTable: "GroceryStore",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_GroceryItemGroceryStoreRelation_GroceryStoreId",
            table: "GroceryItemGroceryStoreRelation",
            column: "GroceryStoreId");

        migrationBuilder.CreateIndex(
            name: "IX_GroceryStore_Name",
            table: "GroceryStore",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GroceryItemGroceryStoreRelation");

        migrationBuilder.DropTable(
            name: "GroceryStore");
    }
}
