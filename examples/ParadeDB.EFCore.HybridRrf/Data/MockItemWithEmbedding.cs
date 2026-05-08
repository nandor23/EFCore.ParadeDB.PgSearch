using ParadeDB.EFCore.Shared;

namespace ParadeDB.EFCore.HybridRrf.Data;

public class MockItemWithEmbedding : MockItem
{
    public float[]? Embedding { get; set; }
}
