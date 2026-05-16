using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using Shouldly;

namespace ParadeDB.EFCore.IntegrationTests.Query;

public sealed class ProximityTests : TestBase
{
    [Test]
    public async Task Match_WithInlineProximityQuery_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = context
            .Products.Where(p =>
                EF.Functions.Match(p.Description, Pdb.Proximity("running").Within(1, "shoes"))
            )
            .ToQueryString();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Match_WithVariableProximityQuery_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string token1 = "running";
        string token2 = "shoes";
        int distance = 1;

        var results = context
            .Products.Where(p =>
                EF.Functions.Match(p.Description, Pdb.Proximity(token1).Within(distance, token2))
            )
            .ToQueryString();

        results.ShouldNotBeNull();
    }
}
