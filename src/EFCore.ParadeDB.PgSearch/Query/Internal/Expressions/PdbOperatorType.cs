namespace EFCore.ParadeDB.PgSearch.Query.Internal.Expressions;

internal enum PdbOperatorType
{
    Disjunction,
    Conjunction,
    Phrase,
    Term,
    Function,
}
