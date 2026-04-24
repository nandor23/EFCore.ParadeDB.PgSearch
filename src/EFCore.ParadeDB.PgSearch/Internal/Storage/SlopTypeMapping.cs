using EFCore.ParadeDB.PgSearch.Modifiers;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.Storage;

internal sealed class SlopTypeMapping : RelationalTypeMapping
{
    public SlopTypeMapping(Slop slop)
        : base(slop.ToString(), typeof(Slop)) { }

    private SlopTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new SlopTypeMapping(parameters);
    }
}
