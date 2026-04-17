using System.Text.RegularExpressions;
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
    public void Phrase_WithVariableSearchTerm_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, searchTerm))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description ### @\w+
            """
        );
    }

    [Test]
    public void Phrase_WithArrayParameter_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string[] searchTerms = ["running", "shoes"];

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, searchTerms))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description ### @\w+
            """
        );
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
    public void Phrase_WithVariableSearchTermAndBoost_TranslatesToSql()
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

    [Test]
    public void Phrase_WithSlop_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, "running shoes", Pdb.Slop(2)))
            .ToQueryString();

        sql.ShouldContain("p.description ### 'running shoes'::pdb.slop(2)");
    }

    [Test]
    public void Phrase_WithVariableSearchTermAndSlop_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string searchTerm = "running shoes";

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, searchTerm, Pdb.Slop(2)))
            .ToQueryString();

        sql.ShouldMatch(
            """
            p\.description ### @\w+::pdb\.slop\(2\)
            """
        );
    }

    [Test]
    public void Phrase_WithArrayParameterAndSlop_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string[] searchTerms = ["running", "shoes"];

        var sql = context
            .Products.Where(p => EF.Functions.Phrase(p.Description, searchTerms, Pdb.Slop(2)))
            .ToQueryString();

        sql.ShouldMatch(
            $"""p.description ### @\w+::{Regex.Escape(Pdb.Slop(2).ToString())}"""
        );
    }
}
