using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodStuffs.Model.Data.Configurations;

public class GroceryItemConfiguration : IEntityTypeConfiguration<GroceryItem>
{
    public void Configure(EntityTypeBuilder<GroceryItem> entity)
    {
        entity.ToTable(nameof(GroceryItem));

        entity.HasMany(si => si.Recipes)
            .WithMany()
            .UsingEntity<RecipeGroceryItemRelation>();

        entity.HasIndex(si => si.Name)
            .IsUnique();

        entity.HasMany(r => r.StorageLocations)
            .WithMany(i => i.GroceryItems)
            .UsingEntity<Dictionary<string, object>>(
                "GroceryItemStorageLocationRelation",
                r => r.HasOne<StorageLocation>().WithMany().HasForeignKey("StorageLocationId"),
                c => c.HasOne<GroceryItem>().WithMany().HasForeignKey("GroceryItemId"));

        entity.HasMany(r => r.GroceryStores)
            .WithMany(i => i.GroceryItems)
            .UsingEntity<Dictionary<string, object>>(
                "GroceryItemGroceryStoreRelation",
                r => r.HasOne<GroceryStore>().WithMany().HasForeignKey("GroceryStoreId"),
                c => c.HasOne<GroceryItem>().WithMany().HasForeignKey("GroceryItemId"));
    }
}
