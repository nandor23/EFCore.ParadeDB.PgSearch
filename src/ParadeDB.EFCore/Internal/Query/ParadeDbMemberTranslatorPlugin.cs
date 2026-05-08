using Microsoft.EntityFrameworkCore.Query;

namespace ParadeDB.EFCore.Internal.Query;

internal sealed class ParadeDbMemberTranslatorPlugin : IMemberTranslatorPlugin
{
    public IEnumerable<IMemberTranslator> Translators { get; } = [];
}
