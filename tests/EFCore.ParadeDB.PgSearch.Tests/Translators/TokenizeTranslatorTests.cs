using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class TokenizeTranslatorTests
{
    [Test]
    public void TokenizeAsArray_WithLiteral_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.TokenizeAsArray(p.Description, Tokenizer.Literal))
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.literal::text[]");
    }

    [Test]
    public void TokenizeAsArray_WithNgram_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.TokenizeAsArray(p.Description, Tokenizer.Ngram(2, 5)))
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.ngram(2,5)::text[]");
    }

    [Test]
    public void TokenizeAsArray_WithUnicodeAndFilter_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p =>
                EF.Functions.TokenizeAsArray(p.Description, Tokenizer.Unicode(TokenFilter.AsciiFolding))
            )
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.unicode_words('ascii_folding=true')::text[]");
    }

    [Test]
    public void TokenizeAsArray_WithLinderaAndFilter_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p =>
                EF.Functions.TokenizeAsArray(
                    p.Description,
                    Tokenizer.Lindera(LinderaLanguage.Japanese, TokenFilter.Trim)
                )
            )
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.lindera(japanese, 'trim=true')::text[]");
    }

    [Test]
    public void TokenizeAsArray_WithComplexTokenizer_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p =>
                EF.Functions.TokenizeAsArray(
                    p.Description,
                    Tokenizer.NgramPrefixOnly(1, 3, TokenFilter.RemoveShort(2), TokenFilter.Trim)
                )
            )
            .ToQueryString();

        sql.ShouldContain(
            "p.description::pdb.ngram(1, 3, 'prefix_only=true', 'remove_short=2', 'trim=true')::text[]"
        );
    }
}
