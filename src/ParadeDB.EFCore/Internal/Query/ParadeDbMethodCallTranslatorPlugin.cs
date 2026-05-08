using Microsoft.EntityFrameworkCore.Query;
using ParadeDB.EFCore.Internal.Query.Translators;

namespace ParadeDB.EFCore.Internal.Query;

internal sealed class ParadeDbMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public ParadeDbMethodCallTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
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
