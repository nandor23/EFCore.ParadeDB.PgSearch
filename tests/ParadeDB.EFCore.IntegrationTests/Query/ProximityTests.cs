using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using Shouldly;

namespace ParadeDB.EFCore.IntegrationTests.Query;

public sealed class ProximityTests : TestBase
{
    [Test]
    public async Task Proximity_WithInlineArguments_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Proximity(p.Description, "running", "shoes", 1))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Proximity_WithVariableArguments_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string token1 = "running";
        string token2 = "shoes";
        int distance = 1;

        var results = await context
            .Products.Where(p => EF.Functions.Proximity(p.Description, token1, token2, distance))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Proximity_WithInlineArgumentsAndOrdered_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p =>
                EF.Functions.Proximity(p.Description, "running", "shoes", 1, ordered: true)
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Proximity_WithVariableArgumentsAndOrdered_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string token1 = "running";
        string token2 = "shoes";
        int distance = 1;

        var results = await context
            .Products.Where(p =>
                EF.Functions.Proximity(p.Description, token1, token2, distance, ordered: true)
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
