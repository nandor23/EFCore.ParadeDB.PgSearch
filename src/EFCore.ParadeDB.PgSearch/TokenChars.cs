namespace EFCore.ParadeDB.PgSearch;

[Flags]
public enum TokenChars
{
    Letter = 1,
    Digit = 2,
    Whitespace = 4,
    Punctuation = 8,
    Symbol = 16,
}
