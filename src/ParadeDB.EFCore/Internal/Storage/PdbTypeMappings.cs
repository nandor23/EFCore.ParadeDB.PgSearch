using Microsoft.EntityFrameworkCore.Storage;

namespace ParadeDB.EFCore.Internal.Storage;

internal static class PdbTypeMappings
{
    public static readonly RelationalTypeMapping Boolean = new BoolTypeMapping("boolean");
}
