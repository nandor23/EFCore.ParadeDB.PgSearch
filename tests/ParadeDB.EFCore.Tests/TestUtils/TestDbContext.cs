using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Tests.TestModels;

namespace ParadeDB.EFCore.Tests.TestUtils;

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
            .UseSnakeCaseNamingConvention()
            .Options;
}
