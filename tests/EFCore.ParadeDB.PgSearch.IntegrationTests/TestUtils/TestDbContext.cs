using EFCore.ParadeDB.PgSearch.IntegrationTests.TestModels;

using Microsoft.EntityFrameworkCore;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;

public sealed class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
}
