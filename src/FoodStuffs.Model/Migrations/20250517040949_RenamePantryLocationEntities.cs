using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RenamePantryLocationEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItemPantryLocationRelation_GroceryItem_GroceryItemId",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItemPantryLocationRelation_PantryLocation_PantryLocationId",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_PantryLocation",
            table: "PantryLocation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GroceryItemPantryLocationRelation",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.DropIndex(
            name: "IX_PantryLocation_Name",
            table: "PantryLocation");

        migrationBuilder.DropIndex(
            name: "IX_GroceryItemPantryLocationRelation_PantryLocationId",
            table: "GroceryItemPantryLocationRelation");

        migrationBuilder.RenameTable(
            name: "PantryLocation",
            newName: "StorageLocation");

        migrationBuilder.RenameTable(
            name: "GroceryItemPantryLocationRelation",
            newName: "GroceryItemStorageLocationRelation");

        migrationBuilder.RenameColumn(
            name: "PantryLocationId",
            table: "GroceryItemStorageLocationRelation",
            newName: "StorageLocationId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_StorageLocation",
            table: "StorageLocation",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GroceryItemStorageLocationRelation",
            table: "GroceryItemStorageLocationRelation",
            columns: new[] { "GroceryItemId", "StorageLocationId" });

        migrationBuilder.CreateIndex(
            name: "IX_StorageLocation_Name",
            table: "StorageLocation",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GroceryItemStorageLocationRelation_StorageLocationId",
            table: "GroceryItemStorageLocationRelation",
            column: "StorageLocationId");

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItemStorageLocationRelation_GroceryItem_GroceryItemId",
            table: "GroceryItemStorageLocationRelation",
            column: "GroceryItemId",
            principalTable: "GroceryItem",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItemStorageLocationRelation_StorageLocation_StorageLocationId",
            table: "GroceryItemStorageLocationRelation",
            column: "StorageLocationId",
            principalTable: "StorageLocation",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItemStorageLocationRelation_GroceryItem_GroceryItemId",
            table: "GroceryItemStorageLocationRelation");

        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItemStorageLocationRelation_StorageLocation_StorageLocationId",
            table: "GroceryItemStorageLocationRelation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_StorageLocation",
            table: "StorageLocation");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GroceryItemStorageLocationRelation",
            table: "GroceryItemStorageLocationRelation");

        migrationBuilder.DropIndex(
            name: "IX_StorageLocation_Name",
            table: "StorageLocation");

        migrationBuilder.DropIndex(
            name: "IX_GroceryItemStorageLocationRelation_StorageLocationId",
            table: "GroceryItemStorageLocationRelation");

        migrationBuilder.RenameColumn(
            name: "StorageLocationId",
            table: "GroceryItemStorageLocationRelation",
            newName: "PantryLocationId");

        migrationBuilder.RenameTable(
            name: "StorageLocation",
            newName: "PantryLocation");

        migrationBuilder.RenameTable(
            name: "GroceryItemStorageLocationRelation",
            newName: "GroceryItemPantryLocationRelation");

        migrationBuilder.AddPrimaryKey(
            name: "PK_PantryLocation",
            table: "PantryLocation",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GroceryItemPantryLocationRelation",
            table: "GroceryItemPantryLocationRelation",
            columns: new[] { "GroceryItemId", "PantryLocationId" });

        migrationBuilder.CreateIndex(
            name: "IX_PantryLocation_Name",
            table: "PantryLocation",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GroceryItemPantryLocationRelation_PantryLocationId",
            table: "GroceryItemPantryLocationRelation",
            column: "PantryLocationId");

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
    }
}
