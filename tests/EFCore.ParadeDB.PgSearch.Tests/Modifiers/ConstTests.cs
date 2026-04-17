using Shouldly;

namespace EFCore.ParadeDB.PgSearch.Tests.Modifiers;

public sealed class ConstTests
{
    [Test]
    public void ToString_ProducesCorrectSql()
    {
        Pdb.Const(2.5f).ToString().ShouldBe("pdb.const(2.5)");
    }
}
