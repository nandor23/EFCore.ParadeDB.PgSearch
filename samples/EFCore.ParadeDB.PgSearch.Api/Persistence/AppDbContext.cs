using EFCore.ParadeDB.PgSearch.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ParadeDB.PgSearch.Api.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("pg_search");

        modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.Entity<Product>().HasKey(p => p.Id);

        modelBuilder
            .Entity<Product>()
            .HasData(
                new Product
                {
                    Id = 1,
                    Name = "Running Shoes",
                    Description = "Lightweight shoes designed for everyday running",
                },
                new Product
                {
                    Id = 2,
                    Name = "Mechanical Keyboard",
                    Description = "Durable keyboard with tactile switches and RGB lighting",
                },
                new Product
                {
                    Id = 3,
                    Name = "Noise Cancelling Headphones",
                    Description = "Over-ear headphones with active noise cancellation",
                },
                new Product
                {
                    Id = 4,
                    Name = "Smart Watch",
                    Description = "Fitness-focused smartwatch with heart rate monitor",
                }
            );
    }
}
