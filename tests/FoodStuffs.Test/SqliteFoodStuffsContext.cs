using FoodStuffs.Model.Data;
using FoodStuffs.Model.Data.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;

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
        SqliteConnection connection,
        IDateTimeService dateTimeService,
        ICurrentUserAccessor currentUserAccessor)
        : base(options, dateTimeService, currentUserAccessor)
    {
        _connection = connection;
    }

    public static SqliteFoodStuffsContext Create(string name, IDateTimeService dateTimeService, ICurrentUserAccessor currentUserAccessor)
    {
        var connection = new SqliteConnection($"Data Source={name};Mode=Memory;Cache=Shared");
        connection.Open();

        var options = new DbContextOptionsBuilder<FoodStuffsContext>()
            .UseSqlite(connection)
            .Options;

        var context = new SqliteFoodStuffsContext(options, connection, dateTimeService, currentUserAccessor);
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
