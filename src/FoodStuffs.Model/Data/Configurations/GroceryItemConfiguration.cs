using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class GroceryItemConfiguration : IEntityTypeConfiguration<GroceryItem>
{
    public void Configure(EntityTypeBuilder<GroceryItem> builder)
    {
        builder.ToTable(nameof(GroceryItem));

        builder.HasMany(si => si.Recipes)
            .WithMany()
            .UsingEntity<RecipeGroceryItemRelation>();

        builder.HasIndex(si => si.Name)
            .IsUnique();

        builder.HasMany(r => r.StorageLocations)
            .WithMany(i => i.GroceryItems)
            .UsingEntity<Dictionary<string, object>>(
                "GroceryItemStorageLocationRelation",
                r => r.HasOne<StorageLocation>().WithMany().HasForeignKey("StorageLocationId"),
                c => c.HasOne<GroceryItem>().WithMany().HasForeignKey("GroceryItemId"));

        builder.HasMany(r => r.GroceryStores)
            .WithMany(i => i.GroceryItems)
            .UsingEntity<Dictionary<string, object>>(
                "GroceryItemGroceryStoreRelation",
                r => r.HasOne<GroceryStore>().WithMany().HasForeignKey("GroceryStoreId"),
                c => c.HasOne<GroceryItem>().WithMany().HasForeignKey("GroceryItemId"));
    }
}
