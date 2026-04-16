using EFCore.ParadeDB.PgSearch.Internal.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.ParadeDB.PgSearch.Internal;

internal sealed class PgSearchTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public PgSearchTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
    {
        Translators =
        [
            new OperatorTranslator(sqlExpressionFactory),
            new ScoreTranslator(sqlExpressionFactory),
            new SnippetTranslator(sqlExpressionFactory),
            // new SnippetPositionsTranslator(sqlExpressionFactory),
            new ProximityTranslator(sqlExpressionFactory),
            new ProximityRegexTranslator(sqlExpressionFactory),
            new TokenizerTranslator(sqlExpressionFactory),
            new AliasTranslator(sqlExpressionFactory),
        ];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
