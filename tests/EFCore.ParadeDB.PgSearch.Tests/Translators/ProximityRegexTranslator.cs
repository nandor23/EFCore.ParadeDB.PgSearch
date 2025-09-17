using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class ProximityRegexTranslator : TranslatorTestBase
{
    [Test]
    public void ProximityRegex_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p => EF.Functions.ProximityRegex(p.Description, "[a-z]+", "noise", 2))
            .ToQueryString();

        sql.ShouldContain("""p."Description" @@@ ((pdb.prox_regex('[a-z]+') ## 2) ## 'noise')""");
    }

    [Test]
    public void ProximityRegex_WithMatchLimit_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, "[a-z]+", "noise", 4, 10)
            )
            .ToQueryString();

        sql.ShouldContain(
            """p."Description" @@@ ((pdb.prox_regex('[a-z]+', 10) ## 4) ## 'noise')"""
        );
    }

    [Test]
    public void ProximityRegex_WhenCalledWithParameters_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        string pattern = "pdb.*search";
        string token = "baz";
        int maxDistance = 6;

        var sql = context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, pattern, token, maxDistance)
            )
            .ToQueryString();

        sql.ShouldContain(
            """p."Description" @@@ ((pdb.prox_regex(@__pattern_1) ## @__maxDistance_3) ## @__token_2)"""
        );
    }

    [Test]
    public void ProximityRegex_WithMatchLimitAndParameters_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        string pattern = "\\d+";
        string token = "unit";
        int maxDistance = 3;
        int matchLimit = 7;

        var sql = context
            .Products.Where(p =>
                EF.Functions.ProximityRegex(p.Description, pattern, token, maxDistance, matchLimit)
            )
            .ToQueryString();

        sql.ShouldContain(
            """p."Description" @@@ ((pdb.prox_regex(@__pattern_1, @__matchLimit_4) ## @__maxDistance_3) ## @__token_2)"""
        );
    }
}
