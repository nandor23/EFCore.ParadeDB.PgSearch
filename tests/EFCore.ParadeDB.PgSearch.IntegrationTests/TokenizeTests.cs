using EFCore.ParadeDB.PgSearch.IntegrationTests.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class TokenizeTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    private static readonly TokenFilter[] AllFilters =
    [
        TokenFilter.AlphaNumericOnly,
        TokenFilter.AsciiFolding,
        TokenFilter.PreserveCase,
        TokenFilter.RemoveStopwords(StopwordsLanguage.English),
        TokenFilter.Stemmer(StemmerLanguage.English),
        TokenFilter.RemoveLong(100),
        TokenFilter.RemoveShort(2),
        TokenFilter.Trim,
    ];

    private static IEnumerable<Func<TokenFilter[], Tokenizer>> TokenizerBuilders()
    {
        yield return filters => Tokenizer.Unicode(filters);
        yield return filters => Tokenizer.Unicode(false, filters);
        yield return filters => Tokenizer.Unicode(true, filters);
        yield return filters => Tokenizer.LiteralNormalized(filters);
        yield return filters => Tokenizer.Whitespace(filters);
        yield return filters => Tokenizer.Ngram(2, 5, filters);
        yield return filters => Tokenizer.NgramPrefixOnly(2, 5, filters);
        yield return filters => Tokenizer.NgramPositions(3, filters);
        yield return filters => Tokenizer.Simple(filters);
        yield return filters => Tokenizer.RegexPattern(@"\w+", filters);
        yield return filters => Tokenizer.ChineseCompatible(filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Chinese, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Japanese, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Korean, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Chinese, true, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Chinese, false, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Japanese, true, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Japanese, false, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Korean, true, filters);
        yield return filters => Tokenizer.Lindera(LinderaLanguage.Korean, false, filters);
        yield return filters => Tokenizer.Icu(filters);
        yield return filters => Tokenizer.SourceCode(filters);
    }

    public static IEnumerable<Tokenizer> Tokenizers()
    {
        yield return Tokenizer.Literal;

        foreach (var build in TokenizerBuilders())
        {
            yield return build([]);
        }
    }

    public static IEnumerable<Tokenizer> TokenizersWithAllFilters()
    {
        return TokenizerBuilders().Select(build => build(AllFilters));
    }

    /*[Test]
    [MethodDataSource(nameof(Tokenizers))]
    public async Task Tokenize_Predicate_ExecutesSuccessfully(Tokenizer tokenizer)
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Where(p =>
                EF.Functions.Phrase(p.Description, EF.Functions.Tokenize(p.Description, tokenizer))
            )
            .Select(p => p.Description)
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    [MethodDataSource(nameof(TokenizersWithAllFilters))]
    public async Task Tokenize_Predicate_WithAllFilters_ExecutesSuccessfully(Tokenizer tokenizer)
    {
        await using var context = DbFixture.CreateContext();
        
        var results = await context
            .Products.Where(p =>
                EF.Functions.Phrase(p.Description, EF.Functions.Tokenize(p.Description, tokenizer))
            )
            .Select(p => p.Description)
            .ToListAsync();

        results.ShouldNotBeNull();
    }*/

    [Test]
    [MethodDataSource(nameof(Tokenizers))]
    public async Task TokenizeAsArray_Projection_ExecutesSuccessfully(Tokenizer tokenizer)
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.TokenizeAsArray(p.Description, tokenizer))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    [MethodDataSource(nameof(TokenizersWithAllFilters))]
    public async Task TokenizeAsArray_Projection_WithAllFilters_ExecutesSuccessfully(
        Tokenizer tokenizer
    )
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.TokenizeAsArray(p.Description, tokenizer))
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
