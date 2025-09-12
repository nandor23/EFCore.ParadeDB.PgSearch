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

        modelBuilder.Entity<Product>().HasKey(p => p.Id);
    }
}
