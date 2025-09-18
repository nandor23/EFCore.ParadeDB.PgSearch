using EFCore.ParadeDB.PgSearch.Tests.TestModels;

using Microsoft.EntityFrameworkCore;

namespace EFCore.ParadeDB.PgSearch.Tests.TestUtils;

public sealed class TestDbContext : DbContext
{
    public TestDbContext()
        : base(CreateDefaultOptions()) { }

    public DbSet<Product> Products => Set<Product>();

    private static DbContextOptions<TestDbContext> CreateDefaultOptions() =>
        new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql(
                connectionString: "Host=localhost;Database=fake;Username=fake;Password=fake",
                npgsqlOptionsAction: o => o.UsePgSearch()
            )
            .Options;
}
