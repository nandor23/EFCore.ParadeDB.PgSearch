using Microsoft.EntityFrameworkCore;
using ParadeDB.EFCore.Extensions;
using ParadeDB.EFCore.Modifiers;
using ParadeDB.EFCore.Tests.TestUtils;
using Shouldly;
using System.Text.RegularExpressions;

namespace ParadeDB.EFCore.Tests.Translators;

public sealed class RegexTermTranslatorTests
{
    [Test]
    public void RegexTerm_WithInlinePattern_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexTerm(p.Description, @"\w+"))
            .ToQueryString();

        sql.ShouldContain(@"p.description @@@ pdb.regex('\w+')");
    }

    [Test]
    public void RegexTerm_WithVariablePattern_TranslatesToSql()
    {
        using var context = new TestDbContext();

        string pattern = @"\w+";

        var sql = context
            .Products.Where(p => EF.Functions.RegexTerm(p.Description, pattern))
            .ToQueryString();

        sql.ShouldMatch("""p\.description @@@ pdb\.regex\(@\w+\)""");
    }

    [Test]
    public void RegexTerm_WithBoost_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Where(p => EF.Functions.RegexTerm(p.Description, @"\w+", Pdb.Boost(2)))
            .ToQueryString();

        sql.ShouldContain(@"p.description @@@ pdb.regex('\w+')::pdb.boost(2)");
    }
}
