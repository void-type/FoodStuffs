using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VoidCore.EntityFramework;

namespace FoodStuffs.Test;

/// <summary>
/// Runs the real model on an in-memory SQLite database so constraints, relationships, and query translation behave like a relational database.
/// Contexts created with the same name share one database, which lasts until the last of them is disposed.
/// </summary>
public sealed class SqliteFoodStuffsContext : FoodStuffsContext
{
    private readonly SqliteConnection _connection;

    private SqliteFoodStuffsContext(
        DbContextOptions<FoodStuffsContext> options,
        SqliteConnection connection)
        : base(options)
    {
        _connection = connection;
    }

    /// <param name="auditInterceptor">Share one instance across contexts so EF can reuse its internal service provider.</param>
    /// <param name="softDeleteInterceptor">Share one instance across contexts so EF can reuse its internal service provider.</param>
    public static SqliteFoodStuffsContext Create(string name, AuditableSaveChangesInterceptor auditInterceptor, SoftDeletableSaveChangesInterceptor softDeleteInterceptor)
    {
        var connection = new SqliteConnection($"Data Source={name};Mode=Memory;Cache=Shared");
        connection.Open();

        var options = new DbContextOptionsBuilder<FoodStuffsContext>()
            .UseSqlite(connection)
            .AddInterceptors(auditInterceptor, softDeleteInterceptor)
            .Options;

        var context = new SqliteFoodStuffsContext(options, connection);
        context.Database.EnsureCreated();
        return context;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // SQLite can't compare or order DateTimeOffset natively.
        configurationBuilder.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetToBinaryConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // SQLite can't use NEWID() as a column default, and the app always sets the file name.
        modelBuilder.Entity<Image>().Property(i => i.FileName).HasDefaultValueSql(null);
    }

    public override void Dispose()
    {
        base.Dispose();
        _connection.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
