using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> entity)
    {
        entity.ToTable(nameof(Image));

        entity.Property(i => i.FileName)
            .HasDefaultValueSql("NEWID()");

        entity.HasIndex(i => i.FileName)
            .IsUnique();

        entity.OwnsOne(i => i.ImageBlob)
            .ToTable(nameof(ImageBlob))
            .WithOwner();

        entity.Navigation(i => i.ImageBlob)
            .AutoInclude(false);
    }
}
