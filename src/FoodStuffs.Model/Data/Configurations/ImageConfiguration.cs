using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(nameof(Image));

        builder.Property(i => i.FileName)
            .HasDefaultValueSql("NEWID()");

        builder.HasIndex(i => i.FileName)
            .IsUnique();

        builder.OwnsOne(i => i.ImageBlob)
            .ToTable(nameof(ImageBlob))
            .WithOwner();

        builder.Navigation(i => i.ImageBlob)
            .AutoInclude(false);
    }
}
