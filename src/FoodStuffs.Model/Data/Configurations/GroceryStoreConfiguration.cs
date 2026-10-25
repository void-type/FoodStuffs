using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class GroceryStoreConfiguration : IEntityTypeConfiguration<GroceryStore>
{
    public void Configure(EntityTypeBuilder<GroceryStore> entity)
    {
        entity.ToTable(nameof(GroceryStore));

        entity.HasIndex(si => si.Name)
            .IsUnique();
    }
}
