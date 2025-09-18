using EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class MatchTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    [Test]
    public async Task MatchDisjunction_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, "these"))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchDisjunction_WithFuzzy_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, "these", Fuzzy.With(2))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchDisjunction_WithBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, "these", Boost.With(2.3f))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchDisjunction_WithFuzzyAndBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(
                    p.Description,
                    "these",
                    Fuzzy.With(2),
                    Boost.With(2.3f)
                )
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchConjunction_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.MatchConjunction(p.Description, "these"))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchConjunction_WithFuzzy_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(p.Description, "these", Fuzzy.With(2))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchConjunction_WithBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(p.Description, "these", Boost.With(2.3f))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchConjunction_WithFuzzyAndBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(
                    p.Description,
                    "these",
                    Fuzzy.With(2),
                    Boost.With(2.3f)
                )
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
