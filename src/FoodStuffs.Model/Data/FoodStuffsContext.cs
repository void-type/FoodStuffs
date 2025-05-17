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
    public virtual DbSet<GroceryDepartment> GroceryDepartments { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<MealPlan> MealPlans { get; set; } = null!;
    public virtual DbSet<Recipe> Recipes { get; set; } = null!;
    public virtual DbSet<GroceryItem> GroceryItems { get; set; } = null!;
    public virtual DbSet<PantryLocation> PantryLocations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable(nameof(Category));

            entity.HasIndex(si => si.Name)
                .IsUnique();
        });

        modelBuilder.Entity<GroceryDepartment>(entity =>
        {
            entity.ToTable(nameof(GroceryDepartment));

            entity.HasIndex(si => si.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable(nameof(Image));

            entity.Property(i => i.FileName)
                .HasDefaultValueSql("NEWID()");

            entity.HasIndex(i => i.FileName)
                .IsUnique();

            entity.OwnsOne(i => i.ImageBlob)
                .ToTable(nameof(ImageBlob))
                .WithOwner();

            entity.Navigation(i => i.ImageBlob)
                .AutoInclude(false);
        });

        modelBuilder.Entity<MealPlan>(entity => entity.ToTable(nameof(MealPlan)));

        modelBuilder.Entity<MealPlanExcludedGroceryItemRelation>(entity =>
        {
            entity.ToTable(nameof(MealPlanExcludedGroceryItemRelation));

            entity.HasKey(mp => new { mp.MealPlanId, mp.GroceryItemId });

            entity.HasOne<MealPlan>()
                .WithMany(mp => mp.ExcludedGroceryItemRelations)
                .HasForeignKey(mps => mps.MealPlanId);

            entity.HasOne(mps => mps.GroceryItem)
                .WithMany()
                .HasForeignKey(mps => mps.GroceryItemId);
        });

        modelBuilder.Entity<MealPlanRecipeRelation>(entity =>
        {
            entity.ToTable(nameof(MealPlanRecipeRelation));

            entity.HasKey(mp => new { mp.MealPlanId, mp.RecipeId });

            entity.HasOne<MealPlan>()
                .WithMany(mp => mp.RecipeRelations)
                .HasForeignKey(mpr => mpr.MealPlanId);

            entity.HasOne(mpr => mpr.Recipe)
                .WithMany(r => r.MealPlanRelations)
                .HasForeignKey(mpr => mpr.RecipeId);
        });

        modelBuilder.Entity<PantryLocation>(entity =>
        {
            entity.ToTable(nameof(PantryLocation));

            entity.HasIndex(si => si.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable(nameof(Recipe));

            entity.HasOne(r => r.PinnedImage)
                .WithMany()
                .HasForeignKey(r => r.PinnedImageId);

            entity.HasMany(r => r.Images)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId);

            entity.HasMany(r => r.Categories)
                .WithMany(i => i.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeCategoryRelation",
                    r => r.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    c => c.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId"));
        });

        modelBuilder.Entity<RecipeGroceryItemRelation>(entity =>
        {
            entity.ToTable(nameof(RecipeGroceryItemRelation));

            entity.HasKey(mp => new { mp.RecipeId, mp.GroceryItemId });

            entity.HasOne<Recipe>()
                .WithMany(r => r.GroceryItemRelations)
                .HasForeignKey(rsi => rsi.RecipeId);

            entity.HasOne(rsi => rsi.GroceryItem)
                .WithMany()
                .HasForeignKey(rsi => rsi.GroceryItemId);
        });

        modelBuilder.Entity<GroceryItem>(entity =>
        {
            entity.ToTable(nameof(GroceryItem));

            entity.HasMany(si => si.Recipes)
                .WithMany()
                .UsingEntity<RecipeGroceryItemRelation>();

            entity.HasIndex(si => si.Name)
                .IsUnique();

            entity.HasMany(r => r.PantryLocations)
                .WithMany(i => i.GroceryItems)
                .UsingEntity<Dictionary<string, object>>(
                    "GroceryItemPantryLocationRelation",
                    r => r.HasOne<PantryLocation>().WithMany().HasForeignKey("PantryLocationId"),
                    c => c.HasOne<GroceryItem>().WithMany().HasForeignKey("GroceryItemId"));
        });
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
