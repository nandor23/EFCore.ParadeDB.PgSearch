using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class OperatorTranslatorTermTests : TranslatorTestBase
{
    [Test]
    public void Term_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, "running shoes"))
            .ToQueryString();

        sql.ShouldContain("""p."Description" === 'running shoes'""");
    }

    [Test]
    public void Term_WithFuzzy_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, "running shoes", Fuzzy.With(3)))
            .ToQueryString();

        sql.ShouldContain("""p."Description" === 'running shoes'::fuzzy(3)""");
    }

    [Test]
    public void Term_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, "running shoes", Boost.With(2)))
            .ToQueryString();

        sql.ShouldContain("""p."Description" === 'running shoes'::boost(2)""");
    }

    [Test]
    public void Term_WithFuzzyAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p =>
                EF.Functions.Term(p.Description, "running shoes", Fuzzy.With(5), Boost.With(3))
            )
            .ToQueryString();

        sql.ShouldContain("""p."Description" === 'running shoes'::fuzzy(5)::boost(3)""");
    }

    [Test]
    [MethodDataSource(
        typeof(OperatorTestDataSources),
        nameof(OperatorTestDataSources.FuzzyBoostTestData)
    )]
    public void Term_WhenCalledWithParameters_TranslatesToSql(Fuzzy fuzzy, Boost boost)
    {
        using var context = new TestDbContext(CreateOptions());

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Term(p.Description, searchTerm, fuzzy, boost))
            .ToQueryString();

        sql.ShouldContain($"""p."Description" === @__searchTerm_1::{fuzzy}::{boost}""");
    }
}
