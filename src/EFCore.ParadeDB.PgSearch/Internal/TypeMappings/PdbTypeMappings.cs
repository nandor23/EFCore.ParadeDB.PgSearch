using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Internal.TypeMappings;

internal static class PdbTypeMappings
{
    public static readonly RelationalTypeMapping Boolean = new BoolTypeMapping("boolean");
}
