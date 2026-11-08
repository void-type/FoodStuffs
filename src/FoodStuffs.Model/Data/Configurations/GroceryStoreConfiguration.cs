using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class GroceryStoreConfiguration : IEntityTypeConfiguration<GroceryStore>
{
    public void Configure(EntityTypeBuilder<GroceryStore> builder)
    {
        builder.ToTable(nameof(GroceryStore));

        builder.HasIndex(si => si.Name)
            .IsUnique();
    }
}
