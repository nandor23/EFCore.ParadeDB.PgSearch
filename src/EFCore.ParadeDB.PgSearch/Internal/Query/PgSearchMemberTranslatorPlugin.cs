using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Internal.Query;

internal sealed class PgSearchMemberTranslatorPlugin : IMemberTranslatorPlugin
{
    public IEnumerable<IMemberTranslator> Translators { get; } = [];
}
