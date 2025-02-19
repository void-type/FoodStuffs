using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class GroceryDepartmentsAndInventoryQuantity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "GroceryDepartmentId",
            table: "ShoppingItem",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "InventoryQuantity",
            table: "ShoppingItem",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "GroceryDepartment",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GroceryDepartment", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ShoppingItem_GroceryDepartmentId",
            table: "ShoppingItem",
            column: "GroceryDepartmentId");

        migrationBuilder.CreateIndex(
            name: "IX_GroceryDepartment_Name",
            table: "GroceryDepartment",
            column: "Name",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_ShoppingItem_GroceryDepartment_GroceryDepartmentId",
            table: "ShoppingItem",
            column: "GroceryDepartmentId",
            principalTable: "GroceryDepartment",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ShoppingItem_GroceryDepartment_GroceryDepartmentId",
            table: "ShoppingItem");

        migrationBuilder.DropTable(
            name: "GroceryDepartment");

        migrationBuilder.DropIndex(
            name: "IX_ShoppingItem_GroceryDepartmentId",
            table: "ShoppingItem");

        migrationBuilder.DropColumn(
            name: "GroceryDepartmentId",
            table: "ShoppingItem");

        migrationBuilder.DropColumn(
            name: "InventoryQuantity",
            table: "ShoppingItem");
    }
}
