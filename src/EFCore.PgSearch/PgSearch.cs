namespace EFCore.PgSearch;

public static class PgSearch
{
    public static bool Match<TColumn>(TColumn column, string field, string value)
    {
        throw new InvalidOperationException("This method is for use in LINQ queries only");
    }
}
