using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class AddSidesField : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Sides",
            table: "Recipe",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Sides",
            table: "Recipe");
    }
}
