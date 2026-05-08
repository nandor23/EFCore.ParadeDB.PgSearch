using Microsoft.EntityFrameworkCore.Storage;
using ParadeDB.EFCore.Modifiers;

namespace ParadeDB.EFCore.Internal.Storage;

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
