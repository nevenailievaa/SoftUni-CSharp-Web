using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data.Models;

namespace SoftUniBazar.Data
{
    public class BazarDbContext : IdentityDbContext
    {
        //Constructor
        public BazarDbContext(DbContextOptions<BazarDbContext> options)
            : base(options) { }

        //Properties (Tables)
        public DbSet<Ad> Ads { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<AdBuyer> AdsBuyers { get; set; } = null!;

        //Models Creating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AdBuyer>()
                .HasKey(ab => new { ab.AdId, ab.BuyerId });

            modelBuilder
                .Entity<Category>()
                .HasData(new Category()
                {
                    Id = 1,
                    Name = "Books"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Cars"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Clothes"
                },
                new Category()
                {
                    Id = 4,
                    Name = "Home"
                },
                new Category()
                {
                    Id = 5,
                    Name = "Technology"
                });


            base.OnModelCreating(modelBuilder);
        }
    }
}