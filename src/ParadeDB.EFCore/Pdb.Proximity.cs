using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ParadeDB.EFCore;

public static partial class Pdb
{
    public static PdbQuery Proximity(string token) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(Proximity)));

    public static PdbQuery ProximityRegex(string pattern) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ProximityRegex)));

    public static PdbQuery ProximityRegex(string pattern, int maxExpansions) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ProximityRegex)));

    public static PdbQuery ProximityArray(params string[] tokens) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ProximityArray)));

    public static PdbQuery ProximityArray(params PdbQuery[] operands) =>
        throw new InvalidOperationException(CoreStrings.FunctionOnClient(nameof(ProximityArray)));
}
