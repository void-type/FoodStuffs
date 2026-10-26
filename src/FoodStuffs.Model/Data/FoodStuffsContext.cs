using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using VoidCore.EntityFramework;

namespace FoodStuffs.Model.Data;

/// <summary>
/// Audit properties are set by <see cref="VoidCore.EntityFramework.AuditableSaveChangesInterceptor"/> and soft-delete properties by
/// <see cref="VoidCore.EntityFramework.SoftDeletableSaveChangesInterceptor"/>. Both are added where the context is registered.
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

        // Hide soft-deleted recipes from every query. Must be called after the entity configurations.
        modelBuilder.ApplySoftDeleteQueryFilter();
    }
}
