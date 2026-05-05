using System.Runtime.CompilerServices;

namespace K1.Modelling;

public sealed record LegalityMatrix
{
    private readonly LegalityMask[] _rows;

    internal LegalityMatrix(LegalityMask[] rows, int srcDomainSize, int dstDomainSize, double tightness)
    {
        _rows = rows;
        SrcDomainSize = srcDomainSize;
        DstDomainSize = dstDomainSize;
        Tightness = tightness;
    }

    public int SrcDomainSize { get; }

    public int DstDomainSize { get; }

    public double Tightness { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Legal(int srcDomainValueIndex, int dstDomainValueIndex) =>
        _rows[srcDomainValueIndex].Legal(dstDomainValueIndex);
}
