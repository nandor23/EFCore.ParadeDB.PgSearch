using EFCore.ParadeDB.PgSearch.Internal.Query.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Internal.Query;

internal sealed class PgSearchMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public PgSearchMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
    {
        Translators =
        [
            new OperatorTranslator(sqlExpressionFactory),
            new ScoreTranslator(sqlExpressionFactory),
            new SnippetTranslator(sqlExpressionFactory),
            new ProximityTranslator(sqlExpressionFactory),
            new TokenizeTranslator(sqlExpressionFactory),
            new AliasTranslator(sqlExpressionFactory),
        ];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
