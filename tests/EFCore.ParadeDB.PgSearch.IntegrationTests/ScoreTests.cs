using EFCore.ParadeDB.PgSearch.IntegrationTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class ScoreTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    [Test]
    public async Task Score_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p => EF.Functions.Term(p.Description, "rich"))
            .Select(p => new
            {
                p.Id,
                p.Name,
                Score = EF.Functions.Score(p.Description),
            })
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Score_WithoutParadeDbFilter_ShouldThrowInvalidOperationException()
    {
        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await using var context = DbFixture.CreateContext();

            await context
                .Products.Select(p => new
                {
                    p.Id,
                    p.Name,
                    Score = EF.Functions.Score(p.Description),
                })
                .ToListAsync();
        });
    }
}
