using EFCore.ParadeDB.PgSearch.Internal.Modifiers;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.TypeMappings;

internal sealed class ConstTypeMapping : RelationalTypeMapping
{
    public ConstTypeMapping(Const @const)
        : base(@const.ToString(), typeof(Const)) { }

    private ConstTypeMapping(RelationalTypeMappingParameters parameters)
        : base(parameters) { }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
    {
        return new ConstTypeMapping(parameters);
    }
}
