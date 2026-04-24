using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.Storage;

internal sealed class AliasTypeMapping : RelationalTypeMapping
{
    public AliasTypeMapping(string alias)
        : base($"pdb.alias('{alias}')", typeof(string)) { }

    private AliasTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new AliasTypeMapping(parameters);
    }
}
