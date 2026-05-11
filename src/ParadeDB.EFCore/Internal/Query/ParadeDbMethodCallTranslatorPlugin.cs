using ParadeDB.EFCore.Internal.Query.Translators;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;

namespace ParadeDB.EFCore.Internal.Query;

internal sealed class PgSearchMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public PgSearchMethodCallTranslatorPlugin(
        ISqlExpressionFactory sqlExpressionFactory,
        ICurrentDbContext currentDbContext)
    {
        Translators =
        [
            new OperatorTranslator(sqlExpressionFactory),
            new ScoreTranslator(sqlExpressionFactory),
            new SnippetTranslator(sqlExpressionFactory),
            new ProximityTranslator(sqlExpressionFactory),
            new TokenizeTranslator(sqlExpressionFactory),
            new AliasTranslator(sqlExpressionFactory, currentDbContext),
            new RegexTermTranslator(sqlExpressionFactory),
            new RegexPhraseTranslator(sqlExpressionFactory),
            new PhrasePrefixTranslator(sqlExpressionFactory),
        ];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
