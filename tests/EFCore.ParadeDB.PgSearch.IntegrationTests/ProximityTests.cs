using EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class ProximityTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    [Test]
    public async Task Proximity_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Proximity(p.Description, "running", "shoes", 1))
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
