using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocation>
{
    public void Configure(EntityTypeBuilder<StorageLocation> entity)
    {
        entity.ToTable(nameof(StorageLocation));

        entity.HasIndex(si => si.Name)
            .IsUnique();
    }
}
