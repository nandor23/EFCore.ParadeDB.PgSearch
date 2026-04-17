namespace EFCore.ParadeDB.PgSearch.Internal.Expressions;

internal enum PdbOperatorType
{
    Disjunction,
    Conjunction,
    Phrase,
    Term,
    Function,
}
