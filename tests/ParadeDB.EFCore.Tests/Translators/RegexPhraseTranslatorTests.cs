using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Modifiers;
using ParadeDB.EFCore.Tests.TestUtils;
using Shouldly;
using System.Text.RegularExpressions;

namespace ParadeDB.EFCore.Tests.Translators;

public sealed class RegexPhraseTranslatorTests
{
    [Test]
    public void RegexPhrase_WithInlineArray_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexPhrase(p.Description, new[] { @"\w+", @"\d+" }))
            .ToQueryString();

        sql.ShouldContain(@"pdb.regex_phrase(ARRAY['\w+','\d+']::text[])");
    }

    [Test]
    public void RegexPhrase_WithVariableArray_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string[] patterns = [@"\w+", @"\d+"];

        var sql = context
            .Products.Where(p => EF.Functions.RegexPhrase(p.Description, patterns))
            .ToQueryString();

        sql.ShouldContain("pdb.regex_phrase");
        sql.ShouldContain("@");
    }

    [Test]
    public void RegexPhrase_WithSlop_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexPhrase(p.Description, new[] { @"\w+" }, 2u))
            .ToQueryString();

        sql.ShouldContain(@"pdb.regex_phrase(ARRAY['\w+']::text[], 2)");
    }

    [Test]
    public void RegexPhrase_WithSlopAndMaxExpansions_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexPhrase(p.Description, new[] { @"\w+" }, 2u, 5u))
            .ToQueryString();

        sql.ShouldContain(@"pdb.regex_phrase(ARRAY['\w+']::text[], 2, 5)");
    }

    [Test]
    public void RegexPhrase_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexPhrase(p.Description, new[] { @"\w+" }, Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldContain(@"pdb.regex_phrase(ARRAY['\w+']::text[])::pdb.boost(2)");
    }

    [Test]
    public void RegexPhrase_WithSlopAndBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexPhrase(p.Description, new[] { @"\w+" }, 2u, Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldContain(@"pdb.regex_phrase(ARRAY['\w+']::text[], 2)::pdb.boost(2)");
    }

    [Test]
    public void RegexPhrase_WithAllParams_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p =>
                EF.Functions.RegexPhrase(p.Description, new[] { @"\w+" }, 2u, 5u, Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldContain(@"pdb.regex_phrase(ARRAY['\w+']::text[], 2, 5)::pdb.boost(2)");
    }
}
