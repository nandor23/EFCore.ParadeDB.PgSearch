using EFCore.ParadeDB.PgSearch.Internal.Modifiers;

using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.TypeMappings;

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
