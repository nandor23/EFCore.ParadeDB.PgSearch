namespace EFCore.ParadeDB.PgSearch.Internal.Query.Expressions;

internal enum PdbOperatorType
{
    Disjunction,
    Conjunction,
    Phrase,
    Term,
    Function,
}
