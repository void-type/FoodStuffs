using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class RenameGroceryDepartmentEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItem_GroceryDepartment_GroceryDepartmentId",
            table: "GroceryItem");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GroceryDepartment",
            table: "GroceryDepartment");

        migrationBuilder.DropIndex(
            name: "IX_GroceryDepartment_Name",
            table: "GroceryDepartment");

        migrationBuilder.RenameTable(
            name: "GroceryDepartment",
            newName: "GroceryAisle");

        migrationBuilder.RenameColumn(
            name: "GroceryDepartmentId",
            table: "GroceryItem",
            newName: "GroceryAisleId");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItem_GroceryDepartmentId",
            table: "GroceryItem",
            newName: "IX_GroceryItem_GroceryAisleId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GroceryAisle",
            table: "GroceryAisle",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_GroceryAisle_Name",
            table: "GroceryAisle",
            column: "Name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItem_GroceryAisle_GroceryAisleId",
            table: "GroceryItem",
            column: "GroceryAisleId",
            principalTable: "GroceryAisle",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_GroceryItem_GroceryAisle_GroceryAisleId",
            table: "GroceryItem");

        migrationBuilder.DropPrimaryKey(
            name: "PK_GroceryAisle",
            table: "GroceryAisle");

        migrationBuilder.DropIndex(
            name: "IX_GroceryAisle_Name",
            table: "GroceryAisle");

        migrationBuilder.RenameTable(
            name: "GroceryAisle",
            newName: "GroceryDepartment");

        migrationBuilder.RenameColumn(
            name: "GroceryAisleId",
            table: "GroceryItem",
            newName: "GroceryDepartmentId");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItem_GroceryAisleId",
            table: "GroceryItem",
            newName: "IX_GroceryItem_GroceryDepartmentId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_GroceryDepartment",
            table: "GroceryDepartment",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_GroceryDepartment_Name",
            table: "GroceryDepartment",
            column: "Name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_GroceryItem_GroceryDepartment_GroceryDepartmentId",
            table: "GroceryItem",
            column: "GroceryDepartmentId",
            principalTable: "GroceryDepartment",
            principalColumn: "Id");
    }
}
