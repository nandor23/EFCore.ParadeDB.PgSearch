using EFCore.ParadeDB.PgSearch.IntegrationTests.TestModels;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;

public sealed class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("pg_search");

        modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.Entity<Product>().HasKey(p => p.Id);
    }
}
