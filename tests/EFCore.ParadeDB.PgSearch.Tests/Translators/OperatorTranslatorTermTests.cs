using System.Text.RegularExpressions;
using EFCore.ParadeDB.PgSearch.Internal.Modifiers;
using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class OperatorTranslatorTermTests
{
    [Test]
    public void Term_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, "running shoes"))
            .ToQueryString();

        sql.ShouldContain("p.description === 'running shoes'");
    }

    [Test]
    public void Term_WithVariableSearchTerm_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, searchTerm))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description === @\w+
            """
        );
    }

    [Test]
    public void Term_WithFuzzy_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, "running shoes", Pdb.Fuzzy(2)))
            .ToQueryString();

        sql.ShouldContain("p.description === 'running shoes'::pdb.fuzzy(2)");
    }

    [Test]
    public void Term_WithVariableSearchTermAndFuzzy_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, searchTerm, Pdb.Fuzzy(2)))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description === @\w+::pdb\.fuzzy\(2\)
            """
        );
    }

    [Test]
    public void Term_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, "running shoes", Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldContain("p.description === 'running shoes'::pdb.boost(2)");
    }

    [Test]
    public void Term_WithVariableSearchTermAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, searchTerm, Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description === @\w+::pdb\.boost\(2\)
            """
        );
    }

    [Test]
    public void Term_WithFuzzyAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.Term(p.Description, "running shoes", Pdb.Fuzzy(1), Pdb.Boost(3))
            )
            .ToQueryString();

        sql.ShouldContain("p.description === 'running shoes'::pdb.fuzzy(1)::pdb.boost(3)");
    }

    [Test]
    public void Term_WithVariableSearchTermFuzzyAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.Term(p.Description, searchTerm, Pdb.Fuzzy(1), Pdb.Boost(3))
            )
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description === @\w+::pdb\.fuzzy\(1\)::pdb\.boost\(3\)
            """
        );
    }

    [Test]
    [MethodDataSource(
        typeof(OperatorTestDataSources),
        nameof(OperatorTestDataSources.FuzzyBoostTestData)
    )]
    public void Term_WithVariableSearchTermAndModifierParameters_TranslatesToSql(
        Fuzzy fuzzy,
        Boost boost
    )
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var fuzzySql = Regex.Escape(fuzzy.ToString());
        var boostSql = Regex.Escape(boost.ToString());

        var pattern = $"""
            p\.description === @\w+::{fuzzySql}::{boostSql}
            """;

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, searchTerm, fuzzy, boost))
            .ToQueryString();

        sql.ShouldMatch(pattern);
    }
}
