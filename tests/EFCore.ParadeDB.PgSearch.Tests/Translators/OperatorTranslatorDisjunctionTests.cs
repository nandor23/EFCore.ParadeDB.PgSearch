using System.Text.RegularExpressions;
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
    public void MatchDisjunction_WithVariableSearchTerm_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, searchTerm))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description ||| @\w+
            """
        );
    }

    [Test]
    public void MatchDisjunction_WithArrayParameter_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string[] searchTerms = ["running", "shoes"];

        var sql = context
            .Products.Where(p => EF.Functions.MatchDisjunction(p.Description, searchTerms))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description ||| @\w+
            """
        );
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
    public void MatchDisjunction_WithVariableSearchTermAndFuzzy_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, searchTerm, Pdb.Fuzzy(2))
            )
            .ToQueryString();

        sql.ShouldMatch($"""p.description ||| @\w+::{Regex.Escape(Pdb.Fuzzy(2).ToString())}""");
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
    public void MatchDisjunction_WithVariableSearchTermAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, searchTerm, Pdb.Boost(2))
            )
            .ToQueryString();

        sql.ShouldMatch($"""p.description ||| @\w+::{Regex.Escape(Pdb.Boost(2).ToString())}""");
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
                    Pdb.Fuzzy(2),
                    Pdb.Boost(3)
                )
            )
            .ToQueryString();

        sql.ShouldContain("p.description ||| 'running shoes'::pdb.fuzzy(2)::pdb.boost(3)");
    }

    [Test]
    public void MatchDisjunction_WithVariableSearchTermFuzzyAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, searchTerm, Pdb.Fuzzy(2), Pdb.Boost(3))
            )
            .ToQueryString();

        var fuzzySql = Regex.Escape(Pdb.Fuzzy(2).ToString());
        var boostSql = Regex.Escape(Pdb.Boost(3).ToString());

        var pattern = $"""
            p\.description ||| @\w+::{fuzzySql}::{boostSql}
            """;

        sql.ShouldMatch(pattern);
    }

    [Test]
    [MethodDataSource(
        typeof(OperatorTestDataSources),
        nameof(OperatorTestDataSources.FuzzyBoostTestData)
    )]
    public void MatchDisjunction_WithVariableSearchTermAndModifierParameters_TranslatesToSql(
        Fuzzy fuzzy,
        Boost boost
    )
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchDisjunction(p.Description, searchTerm, fuzzy, boost)
            )
            .ToQueryString();

        sql.ShouldMatch(
            $"""p.description ||| @\w+::{Regex.Escape(fuzzy.ToString())}::{Regex.Escape(boost.ToString())}"""
        );
    }
}
