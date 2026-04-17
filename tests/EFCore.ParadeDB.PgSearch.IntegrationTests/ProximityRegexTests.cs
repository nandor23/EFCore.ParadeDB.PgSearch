using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class ProximityRegexTests : TestBase
{
    [Test]
    public async Task ProximityRegex_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.ProximityRegex(p.Description, "ru.*", "shoes", 1))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task ProximityRegex_WithMatchLimit_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, "ru.*", "shoes", 1, 100)
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
