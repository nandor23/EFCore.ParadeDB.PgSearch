using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ParadeDB.EFCore.Internal.Metadata;

internal sealed class ParadeDbRelationalAnnotationProvider : IRelationalAnnotationProvider
{
    private const string ParadeDbPrefix = "ParadeDb:";

    private readonly IRelationalAnnotationProvider _inner;

    public ParadeDbRelationalAnnotationProvider(IRelationalAnnotationProvider inner)
    {
        _inner = inner;
    }

    public IEnumerable<IAnnotation> For(IRelationalModel model, bool designTime)
        => _inner.For(model, designTime);

    public IEnumerable<IAnnotation> For(ITable table, bool designTime)
        => _inner.For(table, designTime);

    public IEnumerable<IAnnotation> For(IColumn column, bool designTime)
        => _inner.For(column, designTime);

    public IEnumerable<IAnnotation> For(IView view, bool designTime)
        => _inner.For(view, designTime);

    public IEnumerable<IAnnotation> For(IViewColumn column, bool designTime)
        => _inner.For(column, designTime);

    public IEnumerable<IAnnotation> For(ISqlQuery sqlQuery, bool designTime)
        => _inner.For(sqlQuery, designTime);

    public IEnumerable<IAnnotation> For(ISqlQueryColumn column, bool designTime)
        => _inner.For(column, designTime);

    public IEnumerable<IAnnotation> For(IStoreFunction function, bool designTime)
        => _inner.For(function, designTime);

    public IEnumerable<IAnnotation> For(IStoreFunctionParameter parameter, bool designTime)
        => _inner.For(parameter, designTime);

    public IEnumerable<IAnnotation> For(IFunctionColumn column, bool designTime)
        => _inner.For(column, designTime);

    public IEnumerable<IAnnotation> For(IStoreStoredProcedure storedProcedure, bool designTime)
        => _inner.For(storedProcedure, designTime);

    public IEnumerable<IAnnotation> For(IStoreStoredProcedureParameter parameter, bool designTime)
        => _inner.For(parameter, designTime);

    public IEnumerable<IAnnotation> For(IStoreStoredProcedureResultColumn column, bool designTime)
        => _inner.For(column, designTime);

    public IEnumerable<IAnnotation> For(IUniqueConstraint constraint, bool designTime)
        => _inner.For(constraint, designTime);

    public IEnumerable<IAnnotation> For(ITableIndex index, bool designTime)
    {
        foreach (var annotation in _inner.For(index, designTime))
        {
            yield return annotation;
        }

        if (!designTime)
        {
            yield break;
        }

        var modelIndex = index.MappedIndexes.FirstOrDefault();
        if (modelIndex is null)
        {
            yield break;
        }

        foreach (var annotation in modelIndex.GetAnnotations())
        {
            if (annotation.Name.StartsWith(ParadeDbPrefix, StringComparison.Ordinal))
            {
                yield return annotation;
            }
        }
    }

    public IEnumerable<IAnnotation> For(IForeignKeyConstraint foreignKey, bool designTime)
        => _inner.For(foreignKey, designTime);

    public IEnumerable<IAnnotation> For(ISequence sequence, bool designTime)
        => _inner.For(sequence, designTime);

    public IEnumerable<IAnnotation> For(ICheckConstraint checkConstraint, bool designTime)
        => _inner.For(checkConstraint, designTime);

    public IEnumerable<IAnnotation> For(ITrigger trigger, bool designTime)
        => _inner.For(trigger, designTime);
}
