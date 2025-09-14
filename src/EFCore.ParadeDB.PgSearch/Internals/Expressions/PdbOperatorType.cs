namespace EFCore.ParadeDB.PgSearch.Internals.Expressions;

internal enum PdbOperatorType
{
    Disjunction,
    Conjunction,
    Phrase,
    Term,
    Function,
}
