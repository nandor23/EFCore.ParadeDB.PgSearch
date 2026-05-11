using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Tests.TestUtils;
using Shouldly;

namespace ParadeDB.EFCore.Tests.Translators;

public sealed class PhrasePrefixTranslatorTests
{
    [Test]
    public void PhrasePrefix_WithInlineArray_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.PhrasePrefix(p.Description, new[] { "run", "shoe" }))
            .ToQueryString();

        sql.ShouldContain("pdb.phrase_prefix(ARRAY['run','shoe']::text[])");
    }

    [Test]
    public void PhrasePrefix_WithVariableArray_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string[] values = ["run", "shoe"];

        var sql = context
            .Products.Where(p => EF.Functions.PhrasePrefix(p.Description, values))
            .ToQueryString();

        sql.ShouldContain("pdb.phrase_prefix");
        sql.ShouldContain("@");
    }

    [Test]
    public void PhrasePrefix_WithMaxExpansions_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.PhrasePrefix(p.Description, new[] { "run" }, 10))
            .ToQueryString();

        sql.ShouldContain("pdb.phrase_prefix(ARRAY['run']::text[], 10)");
    }

    [Test]
    public void PhrasePrefix_WithNullMaxExpansions_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.PhrasePrefix(p.Description, new[] { "run" }, null))
            .ToQueryString();

        sql.ShouldContain("pdb.phrase_prefix(ARRAY['run']::text[])");
    }
}
