using EFCore.ParadeDB.PgSearch.Query.Internal.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Query.Internal;

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
            new ProximityRegexTranslator(sqlExpressionFactory),
            new TokenizeTranslator(sqlExpressionFactory),
            new AliasTranslator(sqlExpressionFactory),
        ];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
