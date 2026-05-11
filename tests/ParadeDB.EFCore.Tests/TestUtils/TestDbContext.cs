using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Tests.TestModels;

namespace ParadeDB.EFCore.Tests.TestUtils;

public sealed class TestDbContext : DbContext
{
    public TestDbContext()
        : base(CreateDefaultOptions()) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().OwnsOne(p => p.Details, d => d.ToJson());
    }

    private static DbContextOptions<TestDbContext> CreateDefaultOptions() =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(
                connectionString: "Host=localhost;Database=fake;Username=fake;Password=fake",
                npgsqlOptionsAction: o => o.UseParadeDb()
            )
            .UseSnakeCaseNamingConvention()
            .Options;
}
