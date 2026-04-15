namespace EFCore.ParadeDB.PgSearch;

public sealed class Tokenizer
{
    private string SqlName { get; }
    private IReadOnlyList<TokenFilter> Filters { get; }

    private Tokenizer(string sqlName, TokenFilter[] filters)
    {
        SqlName = sqlName;
        Filters = filters;
    }

    public override string ToString()
    {
        if (Filters.Count == 0)
        {
            return $"pdb.{SqlName}";
        }

        var args = string.Join(", ", Filters.Select(f => f.ToString()));
        return $"pdb.{SqlName}('{args}')";
    }

    public static Tokenizer Unicode(params TokenFilter[] filters) => new("unicode_words", filters);
}
