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

        sql.ShouldContain("""p."Description" @@@ (('with' ## 3) ## 'noise')""");
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

        sql.ShouldContain(
            """p."Description" @@@ ((@__token1_1 ## @__distance_3) ## @__token2_2)"""
        );
    }
}
