using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Model.Data;

/// <summary>
/// Audit properties are set by <see cref="VoidCore.EntityFramework.AuditableSaveChangesInterceptor"/>, which is added where the context is registered.
/// </summary>
public class FoodStuffsContext : DbContext
{
    public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options)
        : base(options)
    {
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
}
