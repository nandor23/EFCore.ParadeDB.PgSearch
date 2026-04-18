using EFCore.ParadeDB.PgSearch.Extensions;
using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class ProximityTranslatorTests
{
    [Test]
    public void Proximity_WithInlineArguments_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Proximity(p.Description, "with", "noise", 3))
            .ToQueryString();

        sql.ShouldContain("p.description @@@ (('with' ## 3) ## 'noise')");
    }

    [Test]
    public void Proximity_WithInlineArgumentsAndOrdered_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.Proximity(p.Description, "with", "noise", 3, ordered: true)
            )
            .ToQueryString();

        sql.ShouldContain("p.description @@@ (('with' ##> 3) ##> 'noise')");
    }

    [Test]
    public void Proximity_WithVariableArguments_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string token1 = "hello";
        string token2 = "world";
        int distance = 5;

        var sql = context
            .Products.Where(p => EF.Functions.Proximity(p.Description, token1, token2, distance))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description @@@ \(\(@\w+ ## @\w+\) ## @\w+\)
            """
        );
    }

    [Test]
    public void Proximity_WithVariableArgumentsAndOrdered_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string token1 = "hello";
        string token2 = "world";
        int distance = 5;

        var sql = context
            .Products.Where(p =>
                EF.Functions.Proximity(p.Description, token1, token2, distance, ordered: true)
            )
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description @@@ \(\(@\w+ ##> @\w+\) ##> @\w+\)
            """
        );
    }
}
