using EFCore.ParadeDB.PgSearch.Modifiers;

using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.Storage;

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
