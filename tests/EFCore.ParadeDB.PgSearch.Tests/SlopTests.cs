using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests;

public sealed class SlopTests
{
    [Test]
    public void ToString_ProducesCorrectSql()
    {
        Slop.With(3).ToString().ShouldBe("pdb.slop(3)");
    }
}
