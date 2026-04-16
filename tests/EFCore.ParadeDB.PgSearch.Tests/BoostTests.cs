using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests;

public sealed class BoostTests
{
    [Test]
    public void ToString_ProducesCorrectSql()
    {
        Boost.With(2.5f).ToString().ShouldBe("pdb.boost(2.5)");
    }
}
