using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests;

public sealed class TokenFilterTests
{
    [Test]
    public void AlphaNumericOnly_ProducesCorrectSql()
    {
        TokenFilter.AlphaNumericOnly.ToString().ShouldBe("'alpha_num_only=true'");
    }
}
