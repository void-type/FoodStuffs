using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FoodStuffs.Web.Data.EntityFramework;

public partial class FoodStuffsContext : DbContext
{
    public virtual DbSet<Blob> Blobs { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
    public virtual DbSet<Recipe> Recipes { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=FoodStuffs");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blob>(entity =>
        {
            entity.ToTable("Blob");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Image)
                .WithOne(p => p.Blob)
                .HasForeignKey<Blob>(d => d.Id)
                .HasConstraintName("FK_Blob_Image");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.HasOne(d => d.Recipe)
                .WithMany(p => p.Images)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK_Image_Recipe");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.ToTable("Ingredient");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Recipe)
                .WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("FK_Ingredient_Recipe");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("Recipe");

            entity.HasOne(d => d.PinnedImage)
                .WithMany()
                .HasForeignKey(d => d.PinnedImageId)
                .HasConstraintName("FK_Recipe_PinnedImage");

            entity.HasMany(d => d.Categories)
                .WithMany(p => p.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryRecipe",
                    l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId").HasConstraintName("FK_CategoryRecipe_Category"),
                    r => r.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId").HasConstraintName("FK_CategoryRecipe_Recipe"),
                    j =>
                    {
                        j.HasKey("RecipeId", "CategoryId");

                        j.ToTable("CategoryRecipe");
                    });
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
