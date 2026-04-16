using System.Text.RegularExpressions;
using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class OperatorTranslatorConjunctionTests
{
    [Test]
    public void MatchConjunction_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.MatchConjunction(p.Description, "running shoes"))
            .ToQueryString();

        sql.ShouldContain("p.description &&& 'running shoes'");
    }

    [Test]
    public void MatchConjunction_WithFuzzy_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(p.Description, "running shoes", Fuzzy.With(2))
            )
            .ToQueryString();

        sql.ShouldContain("p.description &&& 'running shoes'::pdb.fuzzy(2)");
    }

    [Test]
    public void MatchConjunction_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(p.Description, "running shoes", Boost.With(2))
            )
            .ToQueryString();

        sql.ShouldContain("p.description &&& 'running shoes'::pdb.boost(2)");
    }

    [Test]
    public void MatchConjunction_WithFuzzyAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(
                    p.Description,
                    "running shoes",
                    Fuzzy.With(2),
                    Boost.With(3)
                )
            )
            .ToQueryString();

        sql.ShouldContain("p.description &&& 'running shoes'::pdb.fuzzy(2)::pdb.boost(3)");
    }

    [Test]
    [MethodDataSource(
        typeof(OperatorTestDataSources),
        nameof(OperatorTestDataSources.FuzzyBoostTestData)
    )]
    public void MatchConjunction_WhenCalledWithParameters_TranslatesToSql(Fuzzy fuzzy, Boost boost)
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p =>
                EF.Functions.MatchConjunction(p.Description, searchTerm, fuzzy, boost)
            )
            .ToQueryString();

        sql.ShouldMatch(
            $"""p.description &&& @\w+::{Regex.Escape(fuzzy.ToString())}::{Regex.Escape(boost.ToString())}"""
        );
    }
}
