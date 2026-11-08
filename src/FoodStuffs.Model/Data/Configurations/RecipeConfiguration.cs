using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable(nameof(Recipe));

        builder.HasOne(r => r.PinnedImage)
            .WithMany()
            .HasForeignKey(r => r.PinnedImageId);

        builder.HasMany(r => r.Images)
            .WithOne(i => i.Recipe)
            .HasForeignKey(i => i.RecipeId);

        builder.HasMany(r => r.Categories)
            .WithMany(i => i.Recipes)
            .UsingEntity<Dictionary<string, object>>(
                "RecipeCategoryRelation",
                r => r.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                c => c.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId"));
    }
}
