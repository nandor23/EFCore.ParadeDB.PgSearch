using EFCore.ParadeDB.PgSearch.IntegrationTests.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.IntegrationTests;

public sealed class TokenizeTests
{
    [ClassDataSource<DbFixture>]
    public required DbFixture DbFixture { get; init; }

    [Test]
    public async Task Tokenize_Literal_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Literal))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_Unicode_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Unicode()))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_LiteralNormalized_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p =>
                EF.Functions.Tokenize(p.Description, Tokenizer.LiteralNormalized())
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_Whitespace_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Whitespace()))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_Ngram_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Ngram(2, 5)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_NgramPrefixOnly_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p =>
                EF.Functions.Tokenize(p.Description, Tokenizer.NgramPrefixOnly(2, 5))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_NgramPositions_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.NgramPositions(3)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_Simple_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Simple()))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_RegexPattern_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p =>
                EF.Functions.Tokenize(p.Description, Tokenizer.RegexPattern(@"\w+"))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_ChineseCompatible_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p =>
                EF.Functions.Tokenize(p.Description, Tokenizer.ChineseCompatible())
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    [Arguments(LinderaLanguage.Chinese)]
    [Arguments(LinderaLanguage.Japanese)]
    [Arguments(LinderaLanguage.Korean)]
    public async Task Tokenize_Lindera_ExecutesSuccessfully(LinderaLanguage language)
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Lindera(language)))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    [Arguments(LinderaLanguage.Chinese, true)]
    [Arguments(LinderaLanguage.Chinese, false)]
    [Arguments(LinderaLanguage.Japanese, true)]
    [Arguments(LinderaLanguage.Japanese, false)]
    [Arguments(LinderaLanguage.Korean, true)]
    [Arguments(LinderaLanguage.Korean, false)]
    public async Task Tokenize_Lindera_WithKeepWhitespace_ExecutesSuccessfully(
        LinderaLanguage language,
        bool keepWhitespace
    )
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p =>
                EF.Functions.Tokenize(p.Description, Tokenizer.Lindera(language, keepWhitespace))
            )
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_Icu_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.Icu()))
            .ToListAsync();

        results.ShouldNotBeNull();
    }

    [Test]
    public async Task Tokenize_SourceCode_ExecutesSuccessfully()
    {
        await using var context = DbFixture.CreateContext();

        var results = await context
            .Products.Select(p => EF.Functions.Tokenize(p.Description, Tokenizer.SourceCode()))
            .ToListAsync();

        results.ShouldNotBeNull();
    }
}
