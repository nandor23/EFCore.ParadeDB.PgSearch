namespace EFCore.ParadeDB.PgSearch;

public sealed class TokenFilter
{
    private readonly string _sqlParam;

    private TokenFilter(string sqlParam) => _sqlParam = sqlParam;

    public override string ToString() => _sqlParam;

    public static readonly TokenFilter AlphaNumericOnly = new("alpha_num_only=true");
}
