using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class GroceryAisleConfiguration : IEntityTypeConfiguration<GroceryAisle>
{
    public void Configure(EntityTypeBuilder<GroceryAisle> builder)
    {
        builder.ToTable(nameof(GroceryAisle));

        builder.HasIndex(si => si.Name)
            .IsUnique();
    }
}
