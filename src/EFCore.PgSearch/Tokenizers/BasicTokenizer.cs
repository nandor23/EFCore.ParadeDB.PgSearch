using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EFCore.PgSearch.Tokenizers;

internal sealed class BasicTokenizer : Tokenizer
{
    private readonly SqlExpression _sqlExpression;

    public BasicTokenizer(string type)
    {
        _sqlExpression = new SqlFragmentExpression($"tokenizer => paradedb.tokenizer('{type}')");
    }

    internal override SqlExpression ToSqlExpression()
    {
        return _sqlExpression;
    }
}
