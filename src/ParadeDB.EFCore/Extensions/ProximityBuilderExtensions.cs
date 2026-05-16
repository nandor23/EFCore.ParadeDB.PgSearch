using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ParadeDB.EFCore.Extensions;

public static class PdbQueryExtensions
{
    public static PdbQuery Within(this PdbQuery left, int distance, string right) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Within)));

    public static PdbQuery Within(this PdbQuery left, int distance, PdbQuery right) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Within)));

    public static PdbQuery WithinOrdered(this PdbQuery left, int distance, string right) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(WithinOrdered)));

    public static PdbQuery WithinOrdered(this PdbQuery left, int distance, PdbQuery right) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(WithinOrdered)));
}
