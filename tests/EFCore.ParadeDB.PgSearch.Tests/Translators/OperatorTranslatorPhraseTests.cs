using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class OperatorTranslatorPhraseTests : TranslatorTestBase
{
    [Test]
    public void Phrase_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "running shoes"))
            .ToQueryString();

        sql.ShouldContain("""p."Description" ### 'running shoes'""");
    }

    [Test]
    public void Phrase_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "running shoes", Boost.With(2)))
            .ToQueryString();

        sql.ShouldContain("""p."Description" ### 'running shoes'::boost(2)""");
    }

    [Test]
    public void Phrase_WhenCalledWithParameters_TranslatesToSql()
    {
        using var context = new TestDbContext(CreateOptions());

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, searchTerm, Boost.With(2)))
            .ToQueryString();

        sql.ShouldContain("""p."Description" ### @__searchTerm_1::boost(2)""");
    }
}
