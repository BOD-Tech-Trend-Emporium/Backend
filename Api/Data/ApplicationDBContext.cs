using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.CartToProduct.domain.entity;
using Api.src.Category.domain.entity;
using Api.src.Coupon.domain.entity;
using Api.src.Favorite.domain.entity;
using Api.src.Price.domain.entity;
using Api.src.Product.domain.entity;
using Api.src.Review.domain.entity;
using backend.src.User.domain.entity;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<CartEntity> Cart { get; set; }
        public DbSet<CouponEntity> Coupon { get; set; }
        public DbSet<FavoriteEntity> Favorite { get; set; }
        public DbSet<ProductEntity> Product { get; set; }
        public DbSet<ReviewEntity> Review { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<PriceEntity> Price { get; set; }
        public DbSet<CartToProductEntity> CartToProduct { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            modelBuilder.Entity<CartEntity>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ReviewEntity>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProductEntity>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<FavoriteEntity>()
                .Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<FavoriteEntity>()
                .HasKey(f => new { f.ProductId, f.UserId });
            modelBuilder.Entity<FavoriteEntity>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId);
            modelBuilder.Entity<FavoriteEntity>()
               .HasOne(f => f.User)
               .WithMany(u => u.Favorites)
               .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<PriceEntity>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<CartToProductEntity>()
                .HasKey(cp => new { cp.PriceId, cp.CartId });
            modelBuilder.Entity<CartToProductEntity>()
                .HasOne(cp => cp.Price)
                .WithMany(p => p.CartToProducts)
                .HasForeignKey(cp => cp.PriceId);
            modelBuilder.Entity<CartToProductEntity>()
               .HasOne(cp => cp.Cart)
               .WithMany(c => c.CartToProducts)
               .HasForeignKey(f => f.CartId);


        }
    }
}