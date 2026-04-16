using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class ProximityRegexTranslator
{
    [Test]
    public void ProximityRegex_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.ProximityRegex(p.Description, "[a-z]+", "noise", 2))
            .ToQueryString();

        sql.ShouldContain("p.description @@@ ((pdb.prox_regex('[a-z]+') ## 2) ## 'noise')");
    }

    [Test]
    public void ProximityRegex_WithMatchLimit_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, "[a-z]+", "noise", 4, 10)
            )
            .ToQueryString();

        sql.ShouldContain("p.description @@@ ((pdb.prox_regex('[a-z]+', 10) ## 4) ## 'noise')");
    }

    [Test]
    public void ProximityRegex_WhenCalledWithParameters_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string pattern = "pdb.*search";
        string token = "baz";
        int maxDistance = 6;

        var sql = context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, pattern, token, maxDistance)
            )
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description @@@ \(\(pdb\.prox_regex\(@\w+\) ## @\w+\) ## @\w+\)
            """
        );
    }

    [Test]
    public void ProximityRegex_WithMatchLimitAndParameters_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string pattern = "\\d+";
        string token = "unit";
        int maxDistance = 3;
        int matchLimit = 7;

        var sql = context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, pattern, token, maxDistance, matchLimit)
            )
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description @@@ \(\(pdb\.prox_regex\(@\w+, @\w+\) ## @\w+\) ## @\w+\)
            """
        );
    }
}
