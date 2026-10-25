using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class MealPlanRecipeRelationConfiguration : IEntityTypeConfiguration<MealPlanRecipeRelation>
{
    public void Configure(EntityTypeBuilder<MealPlanRecipeRelation> entity)
    {
        entity.ToTable(nameof(MealPlanRecipeRelation));

        entity.HasKey(mp => new { mp.MealPlanId, mp.RecipeId });

        entity.HasOne<MealPlan>()
            .WithMany(mp => mp.RecipeRelations)
            .HasForeignKey(mpr => mpr.MealPlanId);

        entity.HasOne(mpr => mpr.Recipe)
            .WithMany(r => r.MealPlanRelations)
            .HasForeignKey(mpr => mpr.RecipeId);
    }
}
