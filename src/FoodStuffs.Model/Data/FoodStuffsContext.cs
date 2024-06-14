using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Model.Data;

public partial class FoodStuffsContext : DbContext
{
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<MealPlan> MealPlans { get; set; }
    public virtual DbSet<Recipe> Recipes { get; set; }
    public virtual DbSet<ShoppingItem> ShoppingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable(nameof(Category));
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
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.ToTable(nameof(MealPlan));
        });

        modelBuilder.Entity<MealPlanPantryShoppingItemRelation>(entity =>
        {
            entity.ToTable(nameof(MealPlanPantryShoppingItemRelation));

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
                    "RecipeCategory",
                    r => r.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    c => c.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId"));

            entity.OwnsMany(r => r.Ingredients, rel =>
            {
                rel.ToTable(nameof(RecipeIngredient));
                rel.WithOwner();

                rel.Property(d => d.Quantity)
                    .HasPrecision(18, 2);
            });
        });

        modelBuilder.Entity<RecipeShoppingItemRelation>(entity =>
        {
            entity.ToTable(nameof(RecipeShoppingItemRelation));

            entity.HasOne<Recipe>()
                .WithMany(r => r.ShoppingItemRelations)
                .HasForeignKey(rsi => rsi.RecipeId);

            entity.HasOne(rsi => rsi.ShoppingItem)
                .WithMany()
                .HasForeignKey(rsi => rsi.ShoppingItemId);
        });

        modelBuilder.Entity<ShoppingItem>(e =>
        {
            e.ToTable(nameof(ShoppingItem));
        });
    }
}
