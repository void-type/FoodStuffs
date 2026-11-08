using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class MealPlanRecipeRelationConfiguration : IEntityTypeConfiguration<MealPlanRecipeRelation>
{
    public void Configure(EntityTypeBuilder<MealPlanRecipeRelation> builder)
    {
        builder.ToTable(nameof(MealPlanRecipeRelation));

        builder.HasKey(mp => new { mp.MealPlanId, mp.RecipeId });

        builder.HasOne<MealPlan>()
            .WithMany(mp => mp.RecipeRelations)
            .HasForeignKey(mpr => mpr.MealPlanId);

        builder.HasOne(mpr => mpr.Recipe)
            .WithMany(r => r.MealPlanRelations)
            .HasForeignKey(mpr => mpr.RecipeId);
    }
}
