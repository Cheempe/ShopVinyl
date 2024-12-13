using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VinylShop.Web.Data.Entities;

namespace VinylShop.Web.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserEntity, RoleEntity, Guid>(options)
    {
        public DbSet<VinylEntity> Vinyls { get; set; }
        public DbSet<GenreEntity> Genre { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderProductEntity> OrdersProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GenreEntity>()
                .HasMany(e => e.Vinyls)
                .WithOne(e => e.Genre)
                .HasForeignKey(e => e.GenreId);

            modelBuilder.Entity<CartEntity>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(e => e.CartId);

            modelBuilder.Entity<OrderEntity>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Order)
                .HasForeignKey(e => e.OrderId);
        }
    }
}
