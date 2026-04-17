using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class PhraseTests : TestBase
{
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
    public async Task Phrase_WithArrayParameter_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string[] terms = ["these", "shoes"];

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, terms))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Phrase_WithBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "with", Pdb.Boost(2.5f)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Phrase_WithSlop_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "with", Pdb.Slop(2)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Phrase_WithArrayParameterAndSlop_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        string[] terms = ["these", "shoes"];

        var results = await context
            .Products.Where(p => EF.Functions.Phrase(p.Description, terms, Pdb.Slop(2)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
