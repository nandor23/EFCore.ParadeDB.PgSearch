using Microsoft.EntityFrameworkCore.Query;

namespace ParadeDB.EFCore.Internal.Query;

internal sealed class PgSearchMemberTranslatorPlugin : IMemberTranslatorPlugin
{
    public IEnumerable<IMemberTranslator> Translators { get; } = [];
}
