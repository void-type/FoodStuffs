﻿using DataMigratorV13.OldData.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMigratorV13.OldData.EntityFramework;

public partial class FoodStuffsContext : DbContext
{
    public virtual DbSet<Blob> Blobs { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<MealSet> MealSets { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Recipe> Recipes { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("Recipe");

            entity.HasMany(d => d.Categories)
                .WithMany(p => p.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryRecipe",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId"),
                    l => l.HasOne<Recipe>().WithMany()
                        .HasForeignKey("RecipeId"),
                    j =>
                    {
                        j.HasKey("RecipeId", "CategoryId");
                        j.ToTable("CategoryRecipe");
                    });

            entity.HasOne(d => d.PinnedImage)
                .WithMany()
                .HasForeignKey(d => d.PinnedImageId);

            entity.HasMany(d => d.Images)
                .WithOne(p => p.Recipe)
                .HasForeignKey(d => d.RecipeId);

            entity.OwnsMany(d => d.Ingredients, p =>
            {
                p.ToTable("Ingredient");

                p.WithOwner();
            });
        });

        modelBuilder.Entity<Category>(entity => entity.ToTable("Category"));

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.Property(e => e.FileName)
                .HasDefaultValueSql("NEWID()");

            entity.HasIndex(e => e.FileName)
                .IsUnique();

            entity.HasOne(p => p.Blob)
                .WithOne(d => d.Image)
                .HasForeignKey<Blob>(d => d.Id);
        });

        modelBuilder.Entity<Blob>(entity =>
        {
            entity.ToTable("Blob");

            // This is tech-debt from when we wanted Images and Blobs to share an ID to shortcut downloads by ID.
            // We should eventually make this independent.
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MealSet>(entity =>
        {
            entity.ToTable("MealSet");

            entity.HasMany(d => d.Recipes)
                .WithMany(p => p.MealSets);

            entity.OwnsMany(d => d.PantryIngredients, o => o.ToJson());
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .IsUnicode(false)
                .IsFixedLength();
        });
    }
}
