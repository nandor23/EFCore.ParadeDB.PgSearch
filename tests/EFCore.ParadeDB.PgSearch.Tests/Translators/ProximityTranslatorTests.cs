using EFCore.ParadeDB.PgSearch.Tests.TestUtils;

using Microsoft.EntityFrameworkCore;

using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class ProximityTranslatorTests
{
    [Test]
    public void Proximity_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Proximity(p.Description, "with", "noise", 3))
            .ToQueryString();

        sql.ShouldContain("p.description @@@ (('with' ## 3) ## 'noise')");
    }

    [Test]
    public void Proximity_WhenOrdered_TranslatesToSqlWithOrderedOperator()
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
    public void Proximity_WhenCalledWithParameters_TranslatesToSql()
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
    public void Proximity_WhenCalledWithParametersAndOrdered_TranslatesToSqlWithOrderedOperator()
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
