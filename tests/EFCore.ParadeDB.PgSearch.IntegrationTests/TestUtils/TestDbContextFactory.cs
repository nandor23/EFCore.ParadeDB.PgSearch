using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;

public sealed class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
{
    public TestDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql("Host=localhost;Database=factory;Username=factory;Password=factory")
            .UseSnakeCaseNamingConvention()
            .Options;

        return new TestDbContext(options);
    }
}
