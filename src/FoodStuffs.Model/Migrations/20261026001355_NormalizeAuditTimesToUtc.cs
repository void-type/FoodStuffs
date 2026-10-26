using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodStuffs.Model.Migrations;

/// <summary>
/// Audit times used to be recorded with the server's local offset. They are now always recorded in UTC.
/// SWITCHOFFSET only changes how each value is expressed, never the instant it represents, so no data is lost.
/// </summary>
public partial class NormalizeAuditTimesToUtc : Migration
{
    private static readonly string[] _auditedTables =
    [
        "Category",
        "GroceryAisle",
        "GroceryItem",
        "GroceryStore",
        "Image",
        "MealPlan",
        "Recipe",
        "StorageLocation",
    ];

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var table in _auditedTables)
        {
            migrationBuilder.Sql($"""
                UPDATE [{table}]
                SET [CreatedOn] = SWITCHOFFSET([CreatedOn], '+00:00'),
                    [ModifiedOn] = SWITCHOFFSET([ModifiedOn], '+00:00')
                """);
        }
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // The original local offsets aren't recoverable, but the instants are unchanged.
    }
}
