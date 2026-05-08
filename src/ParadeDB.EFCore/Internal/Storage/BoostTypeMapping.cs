using Microsoft.EntityFrameworkCore.Storage;
using ParadeDB.EFCore.Modifiers;

namespace ParadeDB.EFCore.Internal.Storage;

internal sealed class BoostTypeMapping : RelationalTypeMapping
{
    public BoostTypeMapping(Boost boost)
        : base(boost.ToString(), typeof(Boost)) { }

    private BoostTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new BoostTypeMapping(parameters);
    }
}
