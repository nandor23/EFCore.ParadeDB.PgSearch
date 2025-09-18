using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class SnippetTranslatorTests
{
    [Test]
    public void Snippet_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context.Products.Select(p => EF.Functions.Snippet(p.Description)).ToQueryString();

        sql.ShouldContain("""paradedb.snippet(p."Description", '<b>', '</b>', 150)""");
    }

    [Test]
    public void Snippet_WithMaxNumChars_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Snippet(p.Description, 70))
            .ToQueryString();

        sql.ShouldContain("""paradedb.snippet(p."Description", '<b>', '</b>', 70)""");
    }

    [Test]
    public void Snippet_WithTags_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Snippet(p.Description, "<a>", "</a>"))
            .ToQueryString();

        sql.ShouldContain("""paradedb.snippet(p."Description", '<a>', '</a>', 150)""");
    }

    [Test]
    public void Snippet_WithTagsAndMaxNumChars_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Snippet(p.Description, "<div>", "</div>", 235))
            .ToQueryString();

        sql.ShouldContain("""paradedb.snippet(p."Description", '<div>', '</div>', 235)""");
    }
}
