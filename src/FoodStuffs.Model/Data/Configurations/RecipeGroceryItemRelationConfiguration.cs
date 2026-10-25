using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class RecipeGroceryItemRelationConfiguration : IEntityTypeConfiguration<RecipeGroceryItemRelation>
{
    public void Configure(EntityTypeBuilder<RecipeGroceryItemRelation> entity)
    {
        entity.ToTable(nameof(RecipeGroceryItemRelation));

        entity.HasKey(mp => new { mp.RecipeId, mp.GroceryItemId });

        entity.HasOne<Recipe>()
            .WithMany(r => r.GroceryItemRelations)
            .HasForeignKey(rsi => rsi.RecipeId);

        entity.HasOne(rsi => rsi.GroceryItem)
            .WithMany()
            .HasForeignKey(rsi => rsi.GroceryItemId);
    }
}
