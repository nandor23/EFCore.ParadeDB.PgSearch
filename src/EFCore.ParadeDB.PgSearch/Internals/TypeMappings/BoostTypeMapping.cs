using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internals.TypeMappings;

internal sealed class BoostTypeMapping : RelationalTypeMapping
{
    private static readonly Type BoostType = typeof(Boost);

    public BoostTypeMapping(Boost boost)
        : base(boost.ToString(), BoostType) { }

    private BoostTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new BoostTypeMapping(parameters);
    }
}
