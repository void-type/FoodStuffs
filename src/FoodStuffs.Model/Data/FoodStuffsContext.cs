using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;
using VoidCore.Model.Auth;
using VoidCore.Model.Time;

namespace FoodStuffs.Model.Data;

public class FoodStuffsContext : DbContext
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options, IDateTimeService dateTimeService, ICurrentUserAccessor currentUserAccessor)
        : base(options)
    {
        _dateTimeService = dateTimeService;
        _currentUserAccessor = currentUserAccessor;

        ChangeTracker.LazyLoadingEnabled = false;
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<GroceryAisle> GroceryAisles { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<MealPlan> MealPlans { get; set; } = null!;
    public virtual DbSet<Recipe> Recipes { get; set; } = null!;
    public virtual DbSet<GroceryItem> GroceryItems { get; set; } = null!;
    public virtual DbSet<StorageLocation> StorageLocations { get; set; } = null!;
    public virtual DbSet<GroceryStore> GroceryStores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FoodStuffsContext).Assembly);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
        var user = _currentUserAccessor.GetUser().GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits

        ChangeTracker.Entries().SetAuditableProperties(_dateTimeService, user.Login);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var user = await _currentUserAccessor.GetUser();

        ChangeTracker.Entries().SetAuditableProperties(_dateTimeService, user.Login);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
