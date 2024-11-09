using DataMigratorV14.OldData.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMigratorV14.OldData;

public partial class FoodStuffsContext : DbContext
{
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<MealSet> MealSets { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Recipe> Recipes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity => entity.ToTable("Category"));

        modelBuilder.Entity<MealSet>(entity =>
        {
            entity.ToTable("MealSet");

            entity.OwnsMany(d => d.PantryIngredients, p =>
            {
                p.ToJson();

                p.Property(d => d.Quantity)
                    .HasPrecision(18, 2);
            });
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.Property(e => e.FileName)
                .HasDefaultValueSql("NEWID()");

            entity.HasIndex(e => e.FileName)
                .IsUnique();

            entity.OwnsOne(x => x.ImageBlob, p =>
            {
                p.ToTable("ImageBlob");
                p.WithOwner();
            });
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("Recipe");

            entity.HasOne(d => d.PinnedImage)
                .WithMany()
                .HasForeignKey(d => d.PinnedImageId);

            entity.HasMany(d => d.Images)
                .WithOne(p => p.Recipe)
                .HasForeignKey(d => d.RecipeId);

            entity.OwnsMany(d => d.Ingredients, p =>
            {
                p.ToTable("RecipeIngredient");

                p.Property(d => d.Quantity)
                    .HasPrecision(18, 2);

                p.WithOwner();
            });
        });
    }
}
