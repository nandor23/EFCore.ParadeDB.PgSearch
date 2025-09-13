using EFCore.ParadeDB.PgSearch.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch;

internal sealed class PgSearchTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public PgSearchTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
    {
        Translators = [new OperatorTranslator(sqlExpressionFactory)];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
