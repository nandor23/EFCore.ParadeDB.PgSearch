using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.PgSearch.Tokenizers;

internal sealed class NgramTokenizer : Tokenizer
{
    private readonly int _minGram;
    private readonly int _maxGram;
    private readonly bool _prefixOnly;

    public NgramTokenizer(int minGram, int maxGram, bool prefixOnly)
    {
        _minGram = minGram;
        _maxGram = maxGram;
        _prefixOnly = prefixOnly;
    }

    internal override SqlExpression ToSqlExpression()
    {
        return new SqlFragmentExpression(
            $"tokenizer => paradedb.tokenizer('ngram', min_gram => {_minGram}, max_gram => {_maxGram}, prefix_only => {_prefixOnly})"
        );
    }
}
