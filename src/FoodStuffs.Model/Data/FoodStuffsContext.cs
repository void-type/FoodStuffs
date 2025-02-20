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
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<MealPlan> MealPlans { get; set; } = null!;
    public virtual DbSet<Recipe> Recipes { get; set; } = null!;
    public virtual DbSet<ShoppingItem> ShoppingItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable(nameof(Category));

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

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.ToTable(nameof(MealPlan));
        });

        modelBuilder.Entity<MealPlanPantryShoppingItemRelation>(entity =>
        {
            entity.ToTable(nameof(MealPlanPantryShoppingItemRelation));

            entity.HasKey(mp => new { mp.MealPlanId, mp.ShoppingItemId });

            entity.HasOne<MealPlan>()
                .WithMany(mp => mp.PantryShoppingItemRelations)
                .HasForeignKey(mps => mps.MealPlanId);

            entity.HasOne(mps => mps.ShoppingItem)
                .WithMany()
                .HasForeignKey(mps => mps.ShoppingItemId);
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

            entity.OwnsMany(r => r.Ingredients, rel =>
            {
                rel.ToJson();

                rel.Property(d => d.Quantity)
                    .HasPrecision(18, 2);
            });
        });

        modelBuilder.Entity<RecipeShoppingItemRelation>(entity =>
        {
            entity.ToTable(nameof(RecipeShoppingItemRelation));

            entity.HasKey(mp => new { mp.RecipeId, mp.ShoppingItemId });

            entity.HasOne<Recipe>()
                .WithMany(r => r.ShoppingItemRelations)
                .HasForeignKey(rsi => rsi.RecipeId);

            entity.HasOne(rsi => rsi.ShoppingItem)
                .WithMany()
                .HasForeignKey(rsi => rsi.ShoppingItemId);
        });

        modelBuilder.Entity<ShoppingItem>(entity =>
        {
            entity.ToTable(nameof(ShoppingItem));

            entity.HasMany(si => si.Recipes)
                .WithMany()
                .UsingEntity<RecipeShoppingItemRelation>();

            entity.HasIndex(si => si.Name)
                .IsUnique();
        });
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.Entries().SetAuditableProperties(_dateTimeService, _currentUserAccessor.User.Login);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.Entries().SetAuditableProperties(_dateTimeService, _currentUserAccessor.User.Login);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
