using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class MealPlanExcludedGroceryItemRelationConfiguration : IEntityTypeConfiguration<MealPlanExcludedGroceryItemRelation>
{
    public void Configure(EntityTypeBuilder<MealPlanExcludedGroceryItemRelation> entity)
    {
        entity.ToTable(nameof(MealPlanExcludedGroceryItemRelation));

        entity.HasKey(mp => new { mp.MealPlanId, mp.GroceryItemId });

        entity.HasOne<MealPlan>()
            .WithMany(mp => mp.ExcludedGroceryItemRelations)
            .HasForeignKey(mps => mps.MealPlanId);

        entity.HasOne(mps => mps.GroceryItem)
            .WithMany()
            .HasForeignKey(mps => mps.GroceryItemId);
    }
}
