using System.Runtime.CompilerServices;

namespace K1.Modelling;

public readonly record struct LegalityMask
{
    private readonly ulong[] _words;

    private LegalityMask(ulong[] words)
    {
        _words = words;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Legal(int domainValueIndex)
    {
        int word = domainValueIndex >> 6;
        int bit = domainValueIndex & 63;

        return (_words[word] & (1UL << bit)) != 0;
    }

    internal void SetLegal(int domainValueIndex)
    {
        int word = domainValueIndex >> 6;
        int bit = domainValueIndex & 63;
        _words[word] |= 1UL << bit;
    }

    internal static LegalityMask WithWordCount(int wordCount) => new(new ulong[wordCount]);
}
