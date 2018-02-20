using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Data.Models
{
    public partial class FoodStuffsContext : DbContext
    {
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryRecipe> CategoryRecipe { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<CategoryRecipe>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.CategoryId });

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryRecipe)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryRecipe_Category");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.CategoryRecipe)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryRecipe_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Directions).IsRequired();

                entity.Property(e => e.Ingredients).IsRequired();

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.RecipeCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_UserCreated");

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.RecipeModifiedByUser)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Recipe_UserModified");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("char(128)");

                entity.Property(e => e.Salt).IsRequired();

                entity.Property(e => e.UserName).IsRequired();
            });
        }
    }
}