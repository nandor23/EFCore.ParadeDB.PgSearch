using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class SnippetTests : TestBase
{
    [Test]
    public async Task Snippet_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich"))
            .Select(p => EF.Functions.Snippet(p.Description))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Snippet_WithMaxChars_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich"))
            .Select(p => EF.Functions.Snippet(p.Description, 50))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Snippet_WithTags_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich"))
            .Select(p => EF.Functions.Snippet(p.Description, "<a>", "</a>"))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Snippet_WithTagsAndMaxChars_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich"))
            .Select(p => EF.Functions.Snippet(p.Description, "<a>", "</a>", 50))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Snippet_WithoutParadeDbFilter_ShouldThrowException()
    {
        await using var context = DbFixture.CreateContext();

        await Should.ThrowAsync<Exception>(async () =>
        {
            await context.Products.Select(p => EF.Functions.Snippet(p.Description)).ToListAsync();
        });
    }

    [Test]
    public async Task Snippet_ReturnsNull_WhenNoMatch()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, "your", Pdb.Fuzzy(2)))
            .Select(p => new { p.Id, Description = EF.Functions.Snippet(p.Description) })
            .ToListAsync();

        results.ShouldAllBe(r => r.Description == null);
    }
}
