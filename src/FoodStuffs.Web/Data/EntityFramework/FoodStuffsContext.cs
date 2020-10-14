using FoodStuffs.Model.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodStuffs.Web.Data.EntityFramework
{
    public partial class FoodStuffsContext : DbContext
    {
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blob>? Blob { get; set; }
        public virtual DbSet<Category>? Category { get; set; }
        public virtual DbSet<CategoryRecipe>? CategoryRecipe { get; set; }
        public virtual DbSet<Image>? Image { get; set; }
        public virtual DbSet<Recipe>? Recipe { get; set; }
        public virtual DbSet<User>? User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blob>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Bytes).IsRequired();

                entity.HasOne(d => d.Image)
                    .WithOne(p => p.Blob)
                    .HasForeignKey<Blob>(d => d.Id)
                    .HasConstraintName("FK_Blob_Image");
            });

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

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.ModifiedBy).IsRequired();

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Image)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Recipe");

                entity.HasOne<Recipe>()
                    .WithOne(d => d.PinnedImage)
                    .HasForeignKey<Recipe>(d => d.PinnedImageId)
                    .HasConstraintName("FK_Recipe_PinnedImage");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.CreatedBy).IsRequired();

                entity.Property(e => e.Directions).IsRequired();

                entity.Property(e => e.Ingredients).IsRequired();

                entity.Property(e => e.ModifiedBy).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Salt).IsRequired();

                entity.Property(e => e.UserName).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
