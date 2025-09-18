using EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class PhraseTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    [Test]
    public async Task Phrase_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "with"))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Phrase_WithBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "with", Boost.With(2.5f)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
