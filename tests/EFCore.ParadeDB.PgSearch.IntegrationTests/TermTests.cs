using EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class TermTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    [Test]
    public async Task Term_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich"))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Term_WithFuzzy_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich", Fuzzy.With(2)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Term_WithBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich", Boost.With(2.3f)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Term_WithFuzzyAndBoost_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p =>
                EF.Functions.Term(p.Description, "rich", Fuzzy.With(2), Boost.With(2.3f))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
