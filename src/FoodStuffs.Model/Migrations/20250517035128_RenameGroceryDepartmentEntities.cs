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

        migrationBuilder.DropTable(
            name: "GroceryDepartment");

        migrationBuilder.RenameColumn(
            name: "GroceryDepartmentId",
            table: "GroceryItem",
            newName: "GroceryAisleId");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItem_GroceryDepartmentId",
            table: "GroceryItem",
            newName: "IX_GroceryItem_GroceryAisleId");

        migrationBuilder.CreateTable(
            name: "GroceryAisle",
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
                table.PrimaryKey("PK_GroceryAisle", x => x.Id);
            });

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

        migrationBuilder.DropTable(
            name: "GroceryAisle");

        migrationBuilder.RenameColumn(
            name: "GroceryAisleId",
            table: "GroceryItem",
            newName: "GroceryDepartmentId");

        migrationBuilder.RenameIndex(
            name: "IX_GroceryItem_GroceryAisleId",
            table: "GroceryItem",
            newName: "IX_GroceryItem_GroceryDepartmentId");

        migrationBuilder.CreateTable(
            name: "GroceryDepartment",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Order = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GroceryDepartment", x => x.Id);
            });

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
