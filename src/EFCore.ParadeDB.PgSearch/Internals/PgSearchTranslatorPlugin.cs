using EFCore.ParadeDB.PgSearch.Internals.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Internals;

internal sealed class PgSearchTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public PgSearchTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
    {
        Translators =
        [
            new OperatorTranslator(sqlExpressionFactory),
            new ScoreTranslator(sqlExpressionFactory),
        ];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
