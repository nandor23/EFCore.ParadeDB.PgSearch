using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class OperatorTranslatorPhraseTests
{
    [Test]
    public void Phrase_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "running shoes"))
            .ToQueryString();

        sql.ShouldContain("p.description ### 'running shoes'");
    }

    [Test]
    public void Phrase_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "running shoes", Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldContain("p.description ### 'running shoes'::pdb.boost(2)");
    }

    [Test]
    public void Phrase_WhenCalledWithParameters_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, searchTerm, Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description ### @\w+::pdb\.boost\(2\)
            """
        );
    }
}
