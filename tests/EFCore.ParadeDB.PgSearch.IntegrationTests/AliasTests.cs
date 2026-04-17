using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class AliasTests : TestBase
{
    [Test]
    public async Task Alias_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Items.Where(p =>
                EF.Functions.MatchDisjunction(
                    EF.Functions.Alias(p.Description, "description_simple"),
                    "sleek"
                )
            )
            .Select(p => p.Description)
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
