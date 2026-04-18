using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Query.Internal;

internal sealed class PgSearchMemberTranslatorPlugin : IMemberTranslatorPlugin
{
    public IEnumerable<IMemberTranslator> Translators { get; } = [];
}
