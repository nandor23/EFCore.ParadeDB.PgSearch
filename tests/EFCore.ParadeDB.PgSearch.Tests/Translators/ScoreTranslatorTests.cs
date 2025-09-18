using EFCore.ParadeDB.PgSearch.Tests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Translators;

public sealed class ScoreTranslatorTests
{
    [Test]
    public void Score_TranslatesToSql()
    {
        using var context = new TestDbContext();

        var sql = context
            .Products.Select(p => new { p.Id, Score = EF.Functions.Score(p.Id) })
            .ToQueryString();

        sql.ShouldContain("""paradedb.score(p."Id")""");
    }
}
