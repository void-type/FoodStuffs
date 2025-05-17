using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations
{
    /// <inheritdoc />
    public partial class RenamePantryLocationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroceryItemPantryLocationRelation");

            migrationBuilder.DropTable(
                name: "PantryLocation");

            migrationBuilder.CreateTable(
                name: "StorageLocation",
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
                    table.PrimaryKey("PK_StorageLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroceryItemStorageLocationRelation",
                columns: table => new
                {
                    GroceryItemId = table.Column<int>(type: "int", nullable: false),
                    StorageLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryItemStorageLocationRelation", x => new { x.GroceryItemId, x.StorageLocationId });
                    table.ForeignKey(
                        name: "FK_GroceryItemStorageLocationRelation_GroceryItem_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalTable: "GroceryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroceryItemStorageLocationRelation_StorageLocation_StorageLocationId",
                        column: x => x.StorageLocationId,
                        principalTable: "StorageLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItemStorageLocationRelation_StorageLocationId",
                table: "GroceryItemStorageLocationRelation",
                column: "StorageLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageLocation_Name",
                table: "StorageLocation",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroceryItemStorageLocationRelation");

            migrationBuilder.DropTable(
                name: "StorageLocation");

            migrationBuilder.CreateTable(
                name: "PantryLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PantryLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroceryItemPantryLocationRelation",
                columns: table => new
                {
                    GroceryItemId = table.Column<int>(type: "int", nullable: false),
                    PantryLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroceryItemPantryLocationRelation", x => new { x.GroceryItemId, x.PantryLocationId });
                    table.ForeignKey(
                        name: "FK_GroceryItemPantryLocationRelation_GroceryItem_GroceryItemId",
                        column: x => x.GroceryItemId,
                        principalTable: "GroceryItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroceryItemPantryLocationRelation_PantryLocation_PantryLocationId",
                        column: x => x.PantryLocationId,
                        principalTable: "PantryLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroceryItemPantryLocationRelation_PantryLocationId",
                table: "GroceryItemPantryLocationRelation",
                column: "PantryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PantryLocation_Name",
                table: "PantryLocation",
                column: "Name",
                unique: true);
        }
    }
}
