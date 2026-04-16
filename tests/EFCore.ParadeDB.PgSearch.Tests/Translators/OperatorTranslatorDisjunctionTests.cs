using EFCore.ParadeDB.PgSearch.Internal.Modifiers;
using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class OperatorTranslatorDisjunctionTests
{
    [Test]
    public void MatchDisjunction_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, "running shoes"))
            .ToQueryString();

        sql.ShouldContain("p.description ||| 'running shoes'");
    }

    [Test]
    public void MatchDisjunction_WithFuzzy_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, "running shoes", Pdb.Fuzzy(2))
            )
            .ToQueryString();

        sql.ShouldContain("p.description ||| 'running shoes'::pdb.fuzzy(2)");
    }

    [Test]
    public void MatchDisjunction_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, "running shoes", Pdb.Boost(2))
            )
            .ToQueryString();

        sql.ShouldContain("p.description ||| 'running shoes'::pdb.boost(2)");
    }

    [Test]
    public void MatchDisjunction_WithFuzzyAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(
                    p.Description,
                    "running shoes",
                    Pdb.Fuzzy(1),
                    Pdb.Boost(3)
                )
            )
            .ToQueryString();

        sql.ShouldContain("p.description ||| 'running shoes'::pdb.fuzzy(1)::pdb.boost(3)");
    }

    [Test]
    [MethodDataSource(
        typeof(OperatorTestDataSources),
        nameof(OperatorTestDataSources.FuzzyBoostTestData)
    )]
    public void MatchDisjunction_WhenCalledWithParameters_TranslatesToSql(Fuzzy fuzzy, Boost boost)
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, searchTerm, fuzzy, boost)
            )
            .ToQueryString();

        sql.ShouldMatch(
            $"""
            "description" ||| @\w+::{fuzzy}::{boost}
            """
        );
    }
}
