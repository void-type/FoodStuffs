using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <inheritdoc />
public partial class AddImageFileName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FileExtension",
            table: "Blob");

        migrationBuilder.AddColumn<string>(
            name: "FileName",
            table: "Image",
            type: "nvarchar(450)",
            nullable: false,
            defaultValueSql: "NEWID()");

        migrationBuilder.CreateIndex(
            name: "IX_Image_FileName",
            table: "Image",
            column: "FileName",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Image_FileName",
            table: "Image");

        migrationBuilder.DropColumn(
            name: "FileName",
            table: "Image");

        migrationBuilder.AddColumn<string>(
            name: "FileExtension",
            table: "Blob",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
