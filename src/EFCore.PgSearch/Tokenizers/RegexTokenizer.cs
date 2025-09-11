using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.PgSearch.Tokenizers;

internal sealed class RegexTokenizer : Tokenizer
{
    private readonly string _pattern;

    public RegexTokenizer(string pattern)
    {
        _pattern = pattern;
    }

    internal override SqlExpression ToSqlExpression()
    {
        return new SqlFragmentExpression(
            $"tokenizer => paradedb.tokenizer('regex', pattern => '{_pattern.Replace("'", "''")}')"
        );
    }
}
