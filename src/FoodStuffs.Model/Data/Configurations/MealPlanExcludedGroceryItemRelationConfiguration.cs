using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class MealPlanExcludedGroceryItemRelationConfiguration : IEntityTypeConfiguration<MealPlanExcludedGroceryItemRelation>
{
    public void Configure(EntityTypeBuilder<MealPlanExcludedGroceryItemRelation> builder)
    {
        builder.ToTable(nameof(MealPlanExcludedGroceryItemRelation));

        builder.HasKey(mp => new { mp.MealPlanId, mp.GroceryItemId });

        builder.HasOne<MealPlan>()
            .WithMany(mp => mp.ExcludedGroceryItemRelations)
            .HasForeignKey(mps => mps.MealPlanId);

        builder.HasOne(mps => mps.GroceryItem)
            .WithMany()
            .HasForeignKey(mps => mps.GroceryItemId);
    }
}
