using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ParadeDB.EFCore.Extensions;

namespace ParadeDB.EFCore.IntegrationTests.Persistence;

[ExcludeFromCodeCoverage]
public sealed class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
{
    public TestDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseNpgsql("Host=localhost;Database=factory;Username=factory;Password=factory", o => o.UseParadeDb())
            .UseSnakeCaseNamingConvention()
            .Options;

        return new TestDbContext(options);
    }
}
