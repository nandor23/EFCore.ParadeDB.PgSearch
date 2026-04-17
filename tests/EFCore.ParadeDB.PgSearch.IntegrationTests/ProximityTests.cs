using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class ProximityTests : TestBase
{
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
