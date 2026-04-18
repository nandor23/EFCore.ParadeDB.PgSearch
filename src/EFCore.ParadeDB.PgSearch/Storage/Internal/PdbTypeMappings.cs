using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ParadeDB.PgSearch.Storage.Internal;

internal static class PdbTypeMappings
{
    public static readonly RelationalTypeMapping Boolean = new BoolTypeMapping("boolean");
}
