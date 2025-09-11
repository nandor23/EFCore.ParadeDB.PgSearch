using EFCore.PgSearch.Translators;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.PgSearch;

internal sealed class PgSearchTranslatorPlugin : IMethodCallTranslatorPlugin
{
    public PgSearchTranslatorPlugin(ISqlExpressionFactory sqlExpressionFactory)
    {
        Translators = [new BasicSearchTranslator(), new MatchTranslator(sqlExpressionFactory)];
    }

    public IEnumerable<IMethodCallTranslator> Translators { get; }
}
