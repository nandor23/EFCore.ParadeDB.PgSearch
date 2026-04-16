using EFCore.ParadeDB.PgSearch.IntegrationTests.Persistence;
using EFCore.ParadeDB.PgSearch.IntegrationTests.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class TermTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    public static IEnumerable<Func<TestDbContext, IQueryable<Product>>> Queries()
    {
        yield return context =>
            context.Products.Where(p => EF.Functions.Term(p.Description, "rich"));

        yield return context =>
            context.Products.Where(p => EF.Functions.Term(p.Description, "rich", Fuzzy.With(2)));

        yield return context =>
            context.Products.Where(p => EF.Functions.Term(p.Description, "rich", Boost.With(2.3f)));

        yield return context =>
            context.Products.Where(p =>
                EF.Functions.Term(p.Description, "rich", Fuzzy.With(2), Boost.With(2.3f))
            );
    }

    [Test]
    [MethodDataSource(nameof(Queries))]
    public async Task Term_ExecutesSuccessfully(
        Func<TestDbContext, IQueryable<Product>> queryFactory
    )
    {
        await using var context = DbFixture.CreateContext();

        var results = await queryFactory(context).ToListAsync();

        results.ShouldNotBeNull();
    }
}
