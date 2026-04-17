using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class MatchTests : TestBase
{
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
    public async Task MatchDisjunction_WithMultipleValues_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, new[] { "these", "shoes" }))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchDisjunction_WithArrayParameter_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string[] terms = ["these", "shoes"];

        var results = await context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, terms))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchDisjunction_WithFuzzy_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, "these", Pdb.Fuzzy(2))
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
                EF.Functions.MatchDisjunction(p.Description, "these", Pdb.Boost(2.3f))
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
                EF.Functions.MatchDisjunction(p.Description, "these", Pdb.Fuzzy(2), Pdb.Boost(2.3f))
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
    public async Task MatchConjunction_WithMultipleValues_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.MatchConjunction(p.Description, new[] {"these", "shoes"}))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchConjunction_WithArrayParameter_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string[] terms = ["these", "shoes"];

        var results = await context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, terms))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task MatchConjunction_WithFuzzy_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();
        var results = await context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(p.Description, "these", Pdb.Fuzzy(2))
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
                EF.Functions.MatchConjunction(p.Description, "these", Pdb.Boost(2.3f))
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
                EF.Functions.MatchConjunction(p.Description, "these", Pdb.Fuzzy(2), Pdb.Boost(2.3f))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
