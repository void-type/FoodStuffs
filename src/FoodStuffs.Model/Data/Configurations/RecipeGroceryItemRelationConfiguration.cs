using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class RecipeGroceryItemRelationConfiguration : IEntityTypeConfiguration<RecipeGroceryItemRelation>
{
    public void Configure(EntityTypeBuilder<RecipeGroceryItemRelation> builder)
    {
        builder.ToTable(nameof(RecipeGroceryItemRelation));

        builder.HasKey(mp => new { mp.RecipeId, mp.GroceryItemId });

        builder.HasOne<Recipe>()
            .WithMany(r => r.GroceryItemRelations)
            .HasForeignKey(rsi => rsi.RecipeId);

        builder.HasOne(rsi => rsi.GroceryItem)
            .WithMany()
            .HasForeignKey(rsi => rsi.GroceryItemId);
    }
}
