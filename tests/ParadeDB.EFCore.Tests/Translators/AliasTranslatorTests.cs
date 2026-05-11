using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Tests.TestUtils;
using Shouldly;
using System.Text.RegularExpressions;

namespace ParadeDB.EFCore.Tests.Translators;

public sealed class AliasTranslatorTests
{
    [Test]
    public void Alias_WithInlineAlias_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, "description_simple"))
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.alias('description_simple')");
    }

    [Test]
    public void Alias_WithVariableAlias_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string aliasName = "description_simple";

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, aliasName))
            .ToQueryString();

        sql.ShouldContain("p.description::pdb.alias('description_simple')");
    }

    [Test]
    public void Alias_WithTokenizerType_ResolvesFromModel()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, TokenizerType.Literal))
            .ToQueryString();

        sql.ShouldMatch("""p\.description::pdb\.alias\('description'\)""");
    }

    [Test]
    public void Alias_WithTokenizerTypeAndIndex_ResolvesFromModel()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, TokenizerType.Literal, 0))
            .ToQueryString();

        sql.ShouldMatch("""p\.description::pdb\.alias\('description'\)""");
    }

    [Test]
    public void Alias_WithTokenizerTypeWithSuffix_ResolvesFromModel()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, TokenizerType.Icu))
            .ToQueryString();

        sql.ShouldMatch("""p\.description::pdb\.alias\('description_icu'\)""");
    }

    [Test]
    public void Alias_WithTokenizer_ResolvesFromModel()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, Tokenizer.Literal))
            .ToQueryString();

        sql.ShouldMatch("""p\.description::pdb\.alias\('description'\)""");
    }

    [Test]
    public void Alias_WithTokenizerAndIndex_ResolvesFromModel()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => EF.Functions.Alias(p.Description, Tokenizer.Literal, 0))
            .ToQueryString();

        sql.ShouldMatch("""p\.description::pdb\.alias\('description'\)""");
    }
}
