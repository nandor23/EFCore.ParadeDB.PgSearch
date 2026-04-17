using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Internal;

internal sealed class PgSearchMemberTranslatorPlugin : IMemberTranslatorPlugin
{
    public IEnumerable<IMemberTranslator> Translators { get; } = [];
}
