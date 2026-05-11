using ParadeDB.EFCore.BM25Attributes;

namespace ParadeDB.EFCore;

public enum TokenizerType
{
    Icu,
    Simple,
    Literal,
    LiteralNormalized,
    Whitespace,
    Ngram,
    EdgeNgram,
    Unicode,
    ChineseCompatible,
    Lindera,
    Jieba,
    SourceCode,
    RegexPattern
}

public static class TokenizerTypeMappings
{
    private static readonly Dictionary<TokenizerType, Type> TypeMap = new()
    {
        { TokenizerType.Icu, typeof(BM25IcuTokenizerAttribute) },
        { TokenizerType.Simple, typeof(BM25SimpleTokenizerAttribute) },
        { TokenizerType.Literal, typeof(BM25LiteralTokenizerAttribute) },
        { TokenizerType.LiteralNormalized, typeof(BM25LiteralNormalizedTokenizerAttribute) },
        { TokenizerType.Whitespace, typeof(BM25WhitespaceTokenizerAttribute) },
        { TokenizerType.Ngram, typeof(BM25NgramTokenizerAttribute) },
        { TokenizerType.EdgeNgram, typeof(BM25EdgeNgramTokenizerAttribute) },
        { TokenizerType.Unicode, typeof(BM25UnicodeTokenizerAttribute) },
        { TokenizerType.ChineseCompatible, typeof(BM25ChineseCompatibleTokenizerAttribute) },
        { TokenizerType.Lindera, typeof(BM25LinderaTokenizerAttribute) },
        { TokenizerType.Jieba, typeof(BM25JiebaTokenizerAttribute) },
        { TokenizerType.SourceCode, typeof(BM25SourceCodeTokenizerAttribute) },
        { TokenizerType.RegexPattern, typeof(BM25RegexPatternTokenizerAttribute) },
    };

    public static Type GetAttributeType(TokenizerType tokenizerType) => TypeMap[tokenizerType];
}
