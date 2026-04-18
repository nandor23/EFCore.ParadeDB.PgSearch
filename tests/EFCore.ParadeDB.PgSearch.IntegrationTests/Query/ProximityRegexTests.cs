using EFCore.ParadeDB.PgSearch.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests.Query;

public sealed class ProximityRegexTests : TestBase
{
    [Test]
    public async Task ProximityRegex_WithInlineArguments_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.ProximityRegex(p.Description, "ru.*", "shoes", 1))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task ProximityRegex_WithVariableArguments_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string pattern = "ru.*";
        string token = "shoes";
        int maxDistance = 1;

        var results = await context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, pattern, token, maxDistance)
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task ProximityRegex_WithInlineArgumentsAndMatchLimit_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, "ru.*", "shoes", 1, 100)
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task ProximityRegex_WithVariableArgumentsAndMatchLimit_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string pattern = "ru.*";
        string token = "shoes";
        int maxDistance = 1;
        int matchLimit = 100;

        var results = await context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, pattern, token, maxDistance, matchLimit)
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
